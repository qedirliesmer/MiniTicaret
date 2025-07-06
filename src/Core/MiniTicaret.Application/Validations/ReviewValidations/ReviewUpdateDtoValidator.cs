using FluentValidation;
using MiniTicaret.Application.DTOs.ReviewDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Validations.ReviewValidations;

public class ReviewUpdateDtoValidator:AbstractValidator<ReviewUpdateDto>
{
    public ReviewUpdateDtoValidator()
    {
        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating 1-dən 5-ə qədər olmalıdır.");

        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Rəy məzmunu boş ola bilməz.")
            .MaximumLength(1000).WithMessage("Rəy maksimum 1000 simvol ola bilər.");
    }
}
