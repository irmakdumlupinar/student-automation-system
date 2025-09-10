# 🎓 Student Automation System

Tam fonksiyonel bir **Öğrenci Otomasyon Sistemi**.  
- **Backend:** .NET 9 Web API + PostgreSQL + Identity + JWT  
- **Frontend:** Vue 3 + Vite + Pinia + Vue Router + Axios  
- **Roller:** Admin, Teacher, Student  
- **Modüller:** Öğrenciler, Öğretmenler, Dersler, Kayıtlar (Enrollment), Notlar, Devamsızlık  

---

## 🚀 Özellikler
- Kullanıcı kimlik doğrulama (Identity + JWT)
- Roller bazlı yetkilendirme (Admin, Teacher, Student)
- Öğrenci / öğretmen / kurs yönetimi
- Öğrenci not ve devamsızlık kayıtları
- Swagger UI üzerinden API test imkanı
- Vue arayüzü ile kullanıcı dostu frontend

---

## 🛠️ Teknolojiler
- **Backend:** .NET 9, ASP.NET Core, EF Core, PostgreSQL, Identity
- **Frontend:** Vue 3, Vite, Pinia, Vue Router, Axios
- **Araçlar:** Swagger, DBeaver/pgAdmin

---

## 📦 Kurulum

### 1. Backend
```bash
cd backend/StudentAutomation.Api
dotnet tool update --global dotnet-ef
dotnet restore
dotnet ef migrations add Initial
dotnet ef database update
dotnet run
