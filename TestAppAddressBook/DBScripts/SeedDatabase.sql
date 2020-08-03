USE [AddressBook]  
GO  

INSERT INTO [dbo].[Contacts]  
           ([FirstName], [LastName], [Address])
VALUES  
	('Name1', 'Surname1', 'Street1'), 
	('Name2', 'Surname2','Street2'),
	('Name3', 'Surname3','Street3') 
GO  

INSERT INTO [dbo].[Phones]  
           ([ContactId], [Number], [IsActive])
VALUES  
	('1', '555-55-55', 'Active'), 
	('2', '222-33-44', 'InActive'),
	('2', '777-77-77', 'Active') 
GO 
