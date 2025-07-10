using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugPrevention.Repositories.NganVHH.Validation
{
    /// <summary>
    /// Cho phép null hoặc chuỗi rỗng, nhưng nếu có giá trị thì phải là URL hợp lệ.
    /// </summary>
    public class OptionalUrlAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var str = value as string;

            if (string.IsNullOrWhiteSpace(str))
                return true; // Bỏ qua nếu không có dữ liệu

            // Kiểm tra nếu có giá trị thì phải là URL hợp lệ
            return Uri.TryCreate(str, UriKind.Absolute, out var uriResult) &&
                   (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} phải là một đường link hợp lệ (http hoặc https).";
        }
    }
}
