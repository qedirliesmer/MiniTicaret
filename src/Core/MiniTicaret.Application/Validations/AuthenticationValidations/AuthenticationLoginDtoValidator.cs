using FluentValidation;
using MiniTicaret.Application.DTOs.AuthenticationDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Validations.AuthenticationValidations;

public class AuthenticationLoginDtoValidator:AbstractValidator<AuthenticationLoginDto>
{
    public AuthenticationLoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email boş ola bilməz")
            .EmailAddress().WithMessage("Email formatı düzgün deyil");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifrə boş ola bilməz")
            .MinimumLength(6).WithMessage("Şifrə ən az 6 simvol olmalıdır");
    }
}
