# 🎓 Student Automation System
## 📌 Proje Açıklaması
Sistem; öğrencilerin, öğretmenlerin, derslerin, notların ve devamsızlıkların yönetilmesini sağlamaktadır.  
Rol tabanlı yetkilendirme yapılmıştır:  

| Rol      | Yetkiler |
|----------|----------|
| **Admin**    | Öğrenci/öğretmen ekleme, ders açma, kayıt yönetimi |
| **Teacher**  | Kendi derslerine not ve devamsızlık ekleme |
| **Student**  | Kendi notlarını ve devamsızlıklarını görme |

---

## 🚀 Özellikler
- Kullanıcı kimlik doğrulama (Identity + JWT)
- Roller bazlı yetkilendirme (Admin / Teacher / Student)
- Öğrenci ve öğretmen yönetimi
- Ders açma, öğrenci kayıt/çıkarma
- Not ve devamsızlık yönetimi
- Swagger UI üzerinden API test imkanı
- Vue 3 arayüzü ile kullanıcı dostu frontend

---

## 🛠️ Kullanılan Teknolojiler
- **Backend:** .NET 9, ASP.NET Core, EF Core, PostgreSQL
- **Frontend:** Vue 3, Vite, Pinia, Vue Router, Axios
- **Kimlik Yönetimi:** Identity + JWT
- **Veritabanı Yönetimi:** PostgreSQL (pgAdmin / DBeaver)

---

## ⚙️ Kurulum Adımları

### 1. Veritabanı
- PostgreSQL üzerinde `studentdb` adında veritabanı oluştur.  
- `backend/StudentAutomation.Api/appsettings.Development.json` dosyasında bağlantı bilgilerini düzenle:  
  ```json
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=studentdb;Username=postgres;Password=postgres"
  }

