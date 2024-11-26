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
![image](https://github.com/user-attachments/assets/6a735939-866f-4b4a-b89e-f504d736c07d)
After applying monthly interest
![image](https://github.com/user-attachments/assets/c28419ae-2af6-4dfb-9563-a338c10fd589)
