<div align="center">
  <img src="img/icon.png" alt="Simple Asp .Net Core Identity Template" width="100" height="100">
</div>

## <img src="https://github.com/Anmol-Baranwal/Cool-GIFs-For-GitHub/assets/74038190/29fd6286-4e7b-4d6c-818f-c4765d5e39a9" width="25" style="margin-bottom: -5px;"> About The Project

ASP.NET Core Identity is a membership system that adds login functionality to ASP.NET Core apps. It provides robust features for user authentication, authorization, and managing user accounts. With Identity, you can easily integrate login functionality using various authentication methods like cookies, JWT tokens, or external providers like Google or Facebook. It offers user management capabilities such as user registration, password hashing, account confirmation, and two-factor authentication. Moreover, it seamlessly integrates with Entity Framework Core for data storage, making it highly customizable and extensible. Its flexibility and security make it a preferred choice for building secure and scalable web applications.


## <img src="https://user-images.githubusercontent.com/74038190/212257467-871d32b7-e401-42e8-a166-fcfd7baa4c6b.gif" width ="25" style="margin-bottom: -5px;"> Features

- [x] CRUD Operations
- [x] Migration Operations
- [x] User Authentication
- [x] Register / Login Operations
- [x] Forgot Password
- [x] Roles Operations
- [x] SMTP E-Mail Sender


## <img src="https://media2.giphy.com/media/QssGEmpkyEOhBCb7e1/giphy.gif?cid=ecf05e47a0n3gi1bfqntqmob8g9aid1oyj2wr3ds3mg700bl&rid=giphy.gif" width ="25" style="margin-bottom: -5px;"> Build With

![HTML5](https://img.shields.io/badge/html5-%23E34F26.svg?style=for-the-badge&logo=html5&logoColor=white)
![CSS3](https://img.shields.io/badge/css3-%231572B6.svg?style=for-the-badge&logo=css3&logoColor=white)
![Bootstrap](https://img.shields.io/badge/bootstrap-%238511FA.svg?style=for-the-badge&logo=bootstrap&logoColor=white)
![javascript](https://img.shields.io/badge/javascript%20-%23323330.svg?&style=for-the-badge&logo=javascript&logoColor=%23F7DF1E)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![MySQL](https://img.shields.io/badge/mysql-4479A1.svg?style=for-the-badge&logo=mysql&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![Visual Studio](https://img.shields.io/badge/Visual%20Studio-5C2D91.svg?style=for-the-badge&logo=visual-studio&logoColor=white)

## <img src="https://user-images.githubusercontent.com/74038190/212257465-7ce8d493-cac5-494e-982a-5a9deb852c4b.gif" width ="25" style="margin-bottom: -5px;"> Installation

1. Check the database connection on the appsetting.json file. Customize the database connection path here according to your own computer. By default the database name is IdentityApp. You can enter SMTP settings from your own e-mail service.

   ```json
   {
   "Logging": {
      "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
      }
   },
   "ConnectionStrings": {
      "DefaultConnection": "Server=localhost;Database=IdentityApp;User=;Password=;"
   },
   "EmailSender": {
      "Host": "",
      "Port": 587,
      "EnableSSL": false,
      "Username": "username",
      "Password": "password"
   },
   "AllowedHosts": "*"
   }
   ```
2. Type the add-migration command via the Package Manager Console.
   
   ```
   add-migration DbCreateFirst
   ```

3. Type the update-database command via the Package Manager Console.
   
   ```
   update-database
   ```
4. You can use the information below to enter the admin panel. You can use the /Users/Index address path for the admin panel.
   ```c#
   // --- Identity User Information --- //
   private const string adminUser = "";
   private const string adminPassword = "";
   private const string adminEmailAddress = "";

   // Add new user
   await userManager.CreateAsync(user, adminPassword);
   // Add "admin" role to new user
   await userManager.AddToRoleAsync(user, "admin");
   ```

## <img src="https://user-images.githubusercontent.com/74038190/235294019-40007353-6219-4ec5-b661-b3c35136dd0b.gif" width="30" style="margin-bottom: -5px;"> Contact Information

You can reach out to me using the following contact details:

[![Email](https://img.shields.io/badge/Email-hamzanasir1111.hn%40gmail.com-brightgreen)](mailto:hamzanasir1111.hn@gmail.com.com)

[![LinkedIn](https://img.shields.io/badge/LinkedIn-HamzaNasir-blue)](https://www.linkedin.com/in/hafiz-hamza-nasir-027737185/)

I'm always open to development and collaboration. Feel free to reach out to me!
