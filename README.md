#### Table of Contents
- 1 KKBank - Online banking platform for individuals
  - 1.1 Overview
  - 1.2 Functionalities and Roles
- 2 KKBank BO - Back Office platform for bank employees
  - 2.1 Overview
  - 2.2 Functionalities and Roles
- 3 Architecture
- 4 Technology Stack

# KKBank

KKBank is my defense project for a bachelor's degree in business informatics. 
The software is powered by ASP.NET Core MVC backend and supports a MSSQL database. The project overview, functionalities, architecture and technology stack is reviewed bellow.  
![Screenshot](public/KKBankMenu.png?raw=true "KKBankMenu")

## :pencil: Overview

KKBank is an e-banking platform for individuals.
### Your benefits:
:pushpin: Access your finances :moneybag: anywhere :house_with_garden: :desert_island: :office:, anytime :clock12:, and from any location :earth_americas:. All you need is a device :computer: with an internet connection :electric_plug:.  
:pushpin: Make standard banking operations, online, available 24/7. Transfer money between your account or to other accounts withing KK Bank.  
:pushpin: Apply online for new bank account or for deletion of existing account.  

### How can you register?
Register online in a few simple steps by visiting the KKBank website page. Submit your:  
:pushpin: EGN  
:pushpin: First, Middle and Last Name  
:pushpin: Phone Number  
:pushpin: User Name  
:pushpin: E-mail  
:pushpin: Password  

After you register successfully, you will become a user and will be able to take advantage of the online banking service KKBank. You will be able to:  
:pushpin: Submit request/s for new checking account/s  
:pushpin: Submit request/s for deletion of existing account/s  
:pushpin: Review archive of your requests  
:pushpin: Check your account/s availability/ies  
:pushpin: Transfer money between your accounts in KKBank or to other accounts withing KK Bank  
:pushpin: Check your account transactions  

## :computer: Functionalities and Roles
KKBank as online banking platform, offers only one role - the role of the end customer (Individual).
### :pushpin: Submit request/s for new checking account/s  
![Screenshot](public/addNewAccount.png?raw=true "RequestNewAccount")
### :pushpin: Submit request/s for deletion of existing account/s  
![Screenshot](public/deleteExistingAccount.png?raw=true "DeleteExistingAccount")
### :pushpin: Review archive of your requests  
![Screenshot](public/requestArchive.png?raw=true "RequestArchive")
### :pushpin: Check your account/s availability/ies  
![Screenshot](public/accountBalance.png?raw=true "AccountBalance")
![Screenshot](public/accountBalance1.png?raw=true "AccountBalance1")
### :pushpin: Transfer money between your accounts in KKBank
![Screenshot](public/payments-betweenOwnAccounts.png?raw=true "TransferBetweenOwnAccounts")
### :pushpin: Transfer money to other accounts withing KK Bank  
![Screenshot](public/payments-ToKKBankAccount.png?raw=true "TransferToKKBankAccount")
### :pushpin: Check payment order archive  
![Screenshot](public/paymentOrderArchive.png?raw=true "PaymentOrderArchive")
### :pushpin: Check your account transactions  
![Screenshot](public/transactions.png?raw=true "Transactions")
### :pushpin: Common menus for Request functionalities
![Screenshot](public/requestsMenu.png?raw=true "RequestMenu")
### :pushpin: Common menus for Payment Orders functionalities
![Screenshot](public/paymentOrdersMenu.png?raw=true "PaymentOrdersMenu")

# ![#1589F0]KKBank BO

# :hammer: Architecture
The KKBank and KKBank BO platforms are based on the .NET technology stack.
Both platforms communicate with MSSQL database via service layer.

# :gear: Technology Stack
- C# and .NET 5 MVC
- JS, HTML, CSS, Bootstrap
- Razor View Engine
- MSSQL Server
- TSQL