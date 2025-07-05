using FluentValidation;
using MiniTicaret.Application.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Validations.OrderValidations;

public class OrderProductDetailDtoValidator:AbstractValidator<OrderProductDetailDto>
{
    public OrderProductDetailDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId boş ola bilməz.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title boş ola bilməz.");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("Qiymət 0-dan böyük olmalıdır.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity 0-dan böyük olmalıdır.");
    }
}
