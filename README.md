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

## ⚙️ Kurulum ve Çalıştırma Adımları

### 1. Gereksinimler
- .NET 9 SDK  
- PostgreSQL (pgAdmin veya DBeaver ile yönetilebilir)  
- Node.js ve npm  

### 2. Veritabanı
- PostgreSQL üzerinde `studentdb` adında bir veritabanı oluştur.  
- `backend/StudentAutomation.Api/appsettings.Development.json` dosyasına bağlantı bilgisini yaz:  
  ```json
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=studentdb;Username=postgres;Password=postgres"
  }
backend 
cd backend/StudentAutomation.Api
dotnet tool update --global dotnet-ef
dotnet restore
dotnet ef migrations add Initial
dotnet ef database update
dotnet run
Swagger UI → https://localhost:7086/swagger

frontend
cd frontend
npm install
npm run dev
Arayüz → http://localhost:5173

👤 Kullanıcı Test Bilgileri (Seed Accounts)
	•	Admin
	•	Email: admin@demo.com
	•	Şifre: Admin123*
	•	Teacher
	•	Email: teacher@demo.com
	•	Şifre: Teacher123*
	•	Student
	•	Email: student@demo.com
	•	Şifre: Student123*


