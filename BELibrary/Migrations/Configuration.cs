using System.Security.Cryptography.X509Certificates;
using BELibrary.Core.Utils;

namespace BELibrary.Migrations
{
    using BELibrary.DbContext;
    using BELibrary.Entity;
    using BELibrary.Utils;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<PatientManagementDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PatientManagementDbContext context)
        {
            //Role role = new Role
            //{
            //    Id = Guid.NewGuid(),
            //    RoleEnum = RoleKey.Admin,
            //    Name = RoleKey.GetRole(RoleKey.Admin)
            //};
            //context.Roles.AddOrUpdate(c => c.RoleEnum, role);

            //context.SaveChanges();
            var passwordFactory = VariableExtensions.DefautlPassword + VariableExtensions.KeyCrypto;
            var passwordCrypto = CryptorEngine.Encrypt(passwordFactory, true);
            context.Accounts.AddOrUpdate(c => c.UserName, new Account()
            {
                Id = Guid.NewGuid(),
                FullName = "Quản Trị",
                Phone = "0973 642 032",
                UserName = "admin",
                Password = passwordCrypto,
                Role = 1,
                Gender = true,
                LinkAvatar = ""
            });

            #region Faculty

            var faculties = new List<Faculty>
            {
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Tim mạch Can thiệp"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Tim mạch tổng quát"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Nhịp tim học"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Hồi sức Tim mạch"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Phẫu thuật Tim - Lồng ngực mạch máu"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Nội Tiêu hóa"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Nội Thần kinh tổng quát"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Ngoại Thần kinh"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Nội tiết"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Bệnh lý mạch máu não"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Bệnh Nhiệt đới"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Cơ xương khớp"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Hô hấp"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Ngoại Niệu - Ghép thận"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Nội Thận - Miễn dịch ghép"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Cấp cứu Tổng hợp"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Hồi sức tích cực - Chống độc"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Gây mê - Hồi sức Ngoại"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Ngoại tổng quát"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Ngoại Chấn thương chỉnh hình"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Tai mũi họng"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Răng Hàm Mặt - Mắt"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Y học cổ truyền - Phục hồi chức năng "},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Y học cổ truyền - Phục hồi chức năng"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Y học thể thao"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Khám bệnh"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Khám và Điều trị theo yêu cầu "},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Khám và Điều trị theo yêu cầu"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Xét nghiệm"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Chẩn đoán hình ảnh"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Giải phẫu bệnh"},
                new Faculty {Id = Guid.NewGuid(), Name = "Đơn vị Nội soi"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Dinh dưỡng"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Kiểm soát nhiễm khuẩn"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Dược"},
                new Faculty {Id = Guid.NewGuid(), Name = "Khoa Ung bướu và Y học hạt nhân"},
                new Faculty {Id = Guid.NewGuid(), Name = "Phòng Tổ chức Cán bộ"},
                new Faculty {Id = Guid.NewGuid(), Name = "Phòng Kế hoạch Tổng hợp"},
                new Faculty {Id = Guid.NewGuid(), Name = "Phòng Điều dưỡng"},
                new Faculty {Id = Guid.NewGuid(), Name = "Phòng Chỉ đạo tuyến "},
                new Faculty {Id = Guid.NewGuid(), Name = "Phòng Chỉ đạo tuyến"},
                new Faculty {Id = Guid.NewGuid(), Name = "Phòng Tài chính Kế toán"},
                new Faculty {Id = Guid.NewGuid(), Name = "Phòng Hành chính Quản trị "},
                new Faculty {Id = Guid.NewGuid(), Name = "Phòng Vật tư - Thiết bị y tế"},
                new Faculty {Id = Guid.NewGuid(), Name = "Phòng Công nghệ thông tin"},
                new Faculty {Id = Guid.NewGuid(), Name = "Phòng Quản lý chất lượng "},
                new Faculty {Id = Guid.NewGuid(), Name = "Phòng Công tác xã hội"},
                new Faculty {Id = Guid.NewGuid(), Name = "Nhà Thuốc"}
            };
            //foreach (var faculty in faculties)
            //{
            //    context.Faculties.AddOrUpdate(c => c.Name, faculty);
            //}

            #endregion Faculty

            #region Medical

            var medicines = new List<Medicine>
            {
                new Medicine {Id = Guid.NewGuid(), Name = "Natri chloride 0,45%", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Natri chloride 10%", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Aminosteril N-Hepa 8%", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Abobotulinum Toxin A", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Abobotulinum Toxin A", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Acarbose", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Acenocoumarol", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Acetazolamide", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Acetyl-Dl-Leucine", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Acetylcysteine", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Amoxicillin/ Clavulanic Acid (tiêm)", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Zoledronic Acid", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Acid Thioctic", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Acid Ursodeoxycholic", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Acid Folic", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Acyclovir (Tra mắt)", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Acyclovir (Uống)", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Adalimumab", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Adapalene/ Clindamycin", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Adapalene", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Adenosine", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Adrenaline", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Afatinib", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Albendazole", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Albumin", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Alendronate", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Alendronate", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Alfuzosin", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Alimemazine", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Allopurinol", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Alpha Chymotrypsine (Uống)", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Alpha Chymotrypsine (Tiêm)", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Alprostadil", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Alteplase", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Alverine", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Alverine/ Simethicone", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Alvesin", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Ambroxol", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Amikacin", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Aminophyllin", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Amiodarone", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Amlodipine", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Amphotericin B lipid complex", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Anastrozole", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Atorvastatin", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Atosiban", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Azithromycin (Uống)", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Magnesi Lactate/ Vitamin B6", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Bambuterol", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Levodopa/Benserazide", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Bernevit", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Betahistin", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Bevacizumab", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Bisoprolol", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Mometasone (bôi)", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Broncho Vacxom", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Budesonide (Khí dung)", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Hyoscin-N-Butylbromide", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Mytomycin C", Description = ""},
                new Medicine {Id = Guid.NewGuid(), Name = "Capecitabine", Description = ""}
            };

            //foreach (var medicine in medicines)
            //{
            //    context.Medicines.AddOrUpdate(c => c.Name, medicine);
            //}

            #endregion Medical
        }
    }
}