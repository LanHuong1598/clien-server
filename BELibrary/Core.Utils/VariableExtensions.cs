using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BELibrary.Core.Utils
{
    public static class Common
    {
        public const string Prefix = "BN-";
    }

    public static class FileKey
    {
        public const int MaxLength = 1024 * 1000;

        public static List<string> FileExtensionApprove()
        {
            return new List<string>(new[] { "png", "jpg", "jpeg" });
        }

        public static List<string> FileContentTypeApprove()
        {
            return new List<string>(new[]
            {
                "application/pdf",
                "application/vnd.ms-excel",
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            });
        }
    }

    public static class CookiesKey
    {
        public const string LangCode = "patient_lang_code";
        public const string Admin = "patient_admin_cookies";
        public const string Client = "patient_client_cookies";
    }

    public static class LangCode
    {
        public const string English = "en";
        public const string VietNam = "vi";
        public const string Japan = "ja";
        public const string Default = "en";

        public static string GetText(string code)
        {
            switch (code)
            {
                case "en":
                    return "English";

                case "vi":
                    return "Việt Nam";

                case "ja":
                    return "Japan";

                default:
                    return "Unknown";
            }
        }

        public static List<string> GetList()
        {
            return new List<string>(new[] { "en", "vi", "ja" });
        }

        public static List<SelectListModel> GetDic()
        {
            return new List<SelectListModel>() {
                new SelectListModel{Value="vi",Text="Tiếng việt" },
                new SelectListModel{Value="en",Text="Tiếng anh" },
                new SelectListModel{Value="ja",Text="Tiếng nhật" }
            };
        }
    }

    public static class RoleKey
    {
        public const int Admin = 1;
        public const int Doctor = 2;
        public const int Patient = 3;

        public static List<int> GetList()
        {
            return new List<int>(new[] { 1, 2, 3 });
        }

        public static bool Any(int role)
        {
            return GetList().Any(x => x == role);
        }

        public static List<SelectListModel> GetDic()
        {
            return new List<SelectListModel>() {
                new SelectListModel
                {
                    Value=1,Text="Admin"
                },
                new SelectListModel
                {
                    Value=2,Text="Bác sĩ"
                },
                new SelectListModel
                {
                    Value=3,Text="Bệnh nhân"
                }
            };
        }

        public static string GetRole(int role)
        {
            switch (role)
            {
                case 1:
                    return "Admin";

                case 2:
                    return "Bác sĩ";

                case 3:
                    return "Bệnh nhân";

                default:
                    return "Unknown";
            }
        }
    }

    public static class GenderKey
    {
        public const int Male = 1;
        public const int FeMale = 0;

        public static List<SelectListModel> GetDic()
        {
            return new List<SelectListModel>() {
                new SelectListModel{Value=1,Text="Nam" },
                new SelectListModel{Value=0,Text="Nữ" }
            };
        }

        public static string GetEmployeeType(int type)
        {
            switch (type)
            {
                case 0:
                    return "Nữ";

                case 1:
                    return "Nam";

                default:
                    return "Unknown";
            }
        }
    }

    public static class VariableExtensions
    {
        public static int PageSize = 2;

        public static string KeyCrypto = "#!2020";
        public static string KeyCryptorClient = "#!2020_Client##";
        public static string DefautlPassword = "123qwe";
    }

    public static class StatusMedical
    {
        public const int Hired = 1;
        public const int Availability = 0;
        public const int Unavailable = -1;
        public const int Maintenance = -2;
        public const int Expired = -3;

        public static string GetText(int stt)
        {
            switch (stt)
            {
                case 1:
                    return "Đã mượn";

                case 0:
                    return "Khả dụng";

                case -1:
                    return "Không khả dụng";

                case -2:
                    return "Bảo trì";

                case -3:
                    return "Hết hạn";

                default:
                    return "Unknown";
            }
        }

        public static List<SelectListModel> GetDic()
        {
            return new List<SelectListModel>() {
                new SelectListModel
                {
                    Value=1,Text="Đã mượn"
                },
                new SelectListModel
                {
                    Value=0,Text="Khả dụng"
                },
                new SelectListModel
                {
                    Value=-1,Text="Không khả dụng"
                },
                new SelectListModel
                {
                    Value=-2,Text="Bảo trì"
                },
                new SelectListModel
                {
                    Value=-3,Text="Hết hạn"
                }
            };
        }
    }

    public static class StatusRecord
    {
        public const int InpatientTreatment = 1;
        public const int OutPatientTreatment = 0;

        public static string GetText(int stt)
        {
            switch (stt)
            {
                case 1:
                    return "Điều trị nội trú";

                case 0:
                    return "Điều trị ngoại trú";

                default:
                    return "Unknown";
            }
        }

        public static List<SelectListModel> GetDic()
        {
            return new List<SelectListModel>
                {
                    new SelectListModel
                    {
                        Value = 1, Text = "Điều trị nội trú"
                    },
                    new SelectListModel
                    {
                        Value = 0, Text = "Điều trị ngoại trú"
                    }
                };
        }
    }

    public enum StatusCode
    {
        Success = 200,
        NotFound = 404,
        NotForbidden = 403,
        ServerError = 500
    }

    public static class CountryKey
    {
        public static List<SelectListModel> GetAll()
        {
            List<SelectListModel> lst = new List<SelectListModel>();
            CultureInfo[] cultureInfos = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (var item in cultureInfos)
            {
                RegionInfo regionInfo = new RegionInfo(item.LCID);
                if (!(lst.Any(x => x.Text == regionInfo.EnglishName)))
                {
                    lst.Add(new SelectListModel
                    {
                        Text = regionInfo.EnglishName,
                        Value = regionInfo.EnglishName
                    });
                }
            }
            return lst.OrderBy(o => o.Text).ToList();
        }
    }

    public class SelectListModel
    {
        public object Value { get; set; }
        public string Text { get; set; }
    }
}