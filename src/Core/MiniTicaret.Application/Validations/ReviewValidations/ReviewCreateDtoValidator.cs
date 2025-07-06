using FluentValidation;
using MiniTicaret.Application.DTOs.ReviewDTOs;
using MiniTicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Validations.ReviewValidations;

public class ReviewCreateDtoValidator:AbstractValidator<ReviewCreateDto>
{
    public ReviewCreateDtoValidator()
    {
        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5");

        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Comment is required");
    }
}
