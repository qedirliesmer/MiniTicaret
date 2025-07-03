using FluentValidation;
using MiniTicaret.Application.DTOs.ProductDTOs;
using MiniTicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Validations.ProductValidations;

public class ProductCreateDtoValidator:AbstractValidator<ProductCreateDto>
{
    public ProductCreateDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Başlıq boş ola bilməz.")
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Açıqlama boş ola bilməz.")
            .MaximumLength(1000);

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Qiymət 0-dan böyük olmalıdır.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Kateqoriya seçilməlidir.");

        RuleFor(x => x.ImageUrls)
            .NotNull().WithMessage("Şəkil siyahısı boş ola bilməz.")
            .Must(list => list.Count > 0).WithMessage("Ən az 1 şəkil əlavə olunmalıdır.");
    }
}
