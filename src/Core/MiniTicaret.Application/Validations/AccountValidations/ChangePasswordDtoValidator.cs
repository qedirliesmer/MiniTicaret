using FluentValidation;
using Microsoft.Win32.SafeHandles;
using MiniTicaret.Application.DTOs.AccountDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Validations.AccountValidations;

public class ChangePasswordDtoValidator:AbstractValidator<ChangePasswordDto>
{
    public ChangePasswordDtoValidator()
    {
        RuleFor(x => x.CurrentPassword).NotEmpty();
        RuleFor(x => x.NewPassword).MinimumLength(6).NotEmpty();
        RuleFor(x => x.ConfirmNewPassword)
            .Equal(x => x.NewPassword).WithMessage("Yeni şifrə təsdiqlə ilə eyni olmalıdır.");
    }
}
