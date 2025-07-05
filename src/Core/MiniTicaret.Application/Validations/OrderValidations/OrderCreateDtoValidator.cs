using FluentValidation;
using MiniTicaret.Application.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Validations.OrderValidations;

public class OrderCreateDtoValidator:AbstractValidator<OrderCreateDto>
{
    public OrderCreateDtoValidator()
    {
        RuleFor(x => x.Items)
            .NotNull().WithMessage("Sifariş siyahısı boş olmamalıdır.")
            .Must(x => x.Count > 0).WithMessage("Ən azı bir məhsul seçilməlidir.");

        RuleForEach(x => x.Items).SetValidator(new OrderItemDtoValidator());
    }
}
