using FluentValidation;
using MiniTicaret.Application.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Validations.OrderValidations;

public class OrderListDtoValidator:AbstractValidator<OrderListDto>
{
    public OrderListDtoValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status boş ola bilməz.");

        RuleFor(x => x.ProductCount)
            .GreaterThanOrEqualTo(0).WithMessage("Məhsul sayı mənfi ola bilməz.");

        RuleFor(x => x.TotalPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Ümumi qiymət mənfi ola bilməz.");
    }
}
