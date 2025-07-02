using FluentValidation;
using MiniTicaret.Application.DTOs.AuthenticationDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Validations.AuthenticationValidations;

public class AuthenticationRegisterDtoValidator:AbstractValidator<AuthenticationRegisterDto>
{
    public AuthenticationRegisterDtoValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Ad boş ola bilməz");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email boş ola bilməz")
            .EmailAddress().WithMessage("Email düzgün deyil");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifrə boş ola bilməz")
            .MinimumLength(6).WithMessage("Şifrə minimum 6 simvol olmalıdır");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Rol seçilməlidir");
    }
}
