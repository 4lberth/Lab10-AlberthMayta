using Lab10_AlberthMayta.Application.DTOs;
using Lab10_AlberthMayta.Domain.Ports; 
using Microsoft.Extensions.Configuration; 
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Lab10_AlberthMayta.Infrastructure;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace Lab10_AlberthMayta.Application.usecases
{
    // Esta es tu clase de Caso de Uso
    public class AuthUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        // Inyecta el Puerto (IUnitOfWork) y la Configuración (para leer el JWT Key)
        public AuthUseCase(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        // --- Caso de Uso: REGISTRO ---
        public async Task<UserDto> RegisterAsync(RegisterRequest request)
        {
            // 1. Llama al Puerto de Salida
            var existingUser = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new Exception("El email ya está registrado.");
            }

            // 2. Lógica de Negocio (Encriptación)
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new User
            {
                UserId = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash, // Guardas el hash
                CreatedAt = DateTime.UtcNow
            };
            
            var defaultRole = await _unitOfWork.RoleRepository.GetRoleByNameAsync("User");
            if (defaultRole == null)
            {
                // Esto es un error crítico de configuración de la BBDD
                throw new Exception("El rol 'User' por defecto no existe. Contacte al administrador.");
            }

            // 2. Crear la entidad de la tabla pivote
            var newUserRole = new UserRole
            {
                UserId = newUser.UserId,
                RoleId = defaultRole.RoleId,
                AssignedAt = DateTime.UtcNow
            };

            // 3. Llama al Puerto de Salida para guardar
            await _unitOfWork.UserRepository.AddAsync(newUser);
            await _unitOfWork.UserRoleRepository.AddAsync(newUserRole);
            await _unitOfWork.SaveAsync();

            // 4. Mapeo Manual a DTO
            return new UserDto
            {
                UserId = newUser.UserId,
                Username = newUser.Username,
                Email = newUser.Email
            };
        }

        // --- Caso de Uso: LOGIN ---
        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            // 1. Llama al Puerto de Salida
            var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception("Credenciales inválidas.");
            }

            // 2. Lógica de Negocio (Verificación)
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                throw new Exception("Credenciales inválidas.");
            }
            
            var userRoles = await _unitOfWork.UserRoleRepository.GetRolesByUserIdAsync(user.UserId);
        
            // 2. Cargar los nombres de esos roles
            var roleNames = new List<string>();
            foreach (var userRole in userRoles)
            {
                var role = await _unitOfWork.RoleRepository.GetByIdAsync(userRole.RoleId);
                if (role != null)
                {
                    roleNames.Add(role.RoleName);
                }
            }

            // 3. Lógica de Negocio (Crear Token)
            string token = CreateToken(user, roleNames);

            // 4. Mapeo Manual a DTO
            return new AuthResponse
            {
                UserId = user.UserId,
                Email = user.Email,
                Username = user.Username,
                Token = token
            };
        }
        
        // --- Lógica Auxiliar: Creación de JWT ---
        private string CreateToken(User user, IEnumerable<string> roles) // <-- Recibe los roles
        {
            var jwtKey = _configuration["Jwt:Key"]!;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 3. Crear los "Claims" (información que va dentro del token)
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.Username),
            };

            // 4. AÑADIR LOS ROLES A LOS CLAIMS
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims, // Usamos la lista de claims actualizada
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}