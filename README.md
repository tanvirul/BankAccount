# BankAccount
# Prepare DB
This application uses a Dockerized SQL Server for convenience. If Docker is not installed, please install Docker Desktop. Once installed, navigate to the root folder (`BankAccount`) and run the command:  

```
docker-compose up -d
```
# Prepare App
Navigate to the `BankAccountApp` folder and run the following commands:  

1. `dotnet add package Microsoft.Extensions.FileSystemGlobbing --version 9.0.0`  
2. `dotnet publish -c Release -r win-x64 --self-contained true -o ./publish`  
3. `.\publish\BankAccountApp.exe`  

This will run the application.
Here is the sample output:
![image](https://github.com/user-attachments/assets/6ceee582-ddb7-403e-9304-4cd0832b0705)

After applying monthly interest
![image](https://github.com/user-attachments/assets/6f23aadd-909f-4c99-8a24-9fc8ff52aad2)
