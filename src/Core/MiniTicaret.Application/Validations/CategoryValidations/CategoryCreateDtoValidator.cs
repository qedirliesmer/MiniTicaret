using FluentValidation;
using MiniTicaret.Application.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Validations.CategoryValidations;

public class CategoryCreateDtoValidator:AbstractValidator<CategoryCreateDto>
{
    public CategoryCreateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ad boş ola bilməz")
            .MaximumLength(100).WithMessage("Ad 100 simvoldan çox ola bilməz");
    }
}
