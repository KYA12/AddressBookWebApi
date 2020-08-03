create procedure [dbo].[sp_InsertContact]    
(    
    @FirstName nvarchar(50),    
    @LastName nvarchar(50),    
    @Address nvarchar(50)    
)    
As    
BEGIN    
    
INSERT INTO  dbo.Contacts(FirstName, LastName, Address)   
 VALUES(@FirstName, @LastName, @Address)    
    
END    