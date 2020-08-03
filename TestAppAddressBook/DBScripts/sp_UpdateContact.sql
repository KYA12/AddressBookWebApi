create procedure [dbo].[sp_UpdateContact]   
(    
    @ContactId int,    
    @FirstName varchar(50),    
    @LastName  varchar(50),    
    @Address varchar(50)   
)    
As    
BEGIN    
     UPDATE Contacts    
     SET FirstName =@FirstName,    
     LastName =@LastName,    
     Address = @Address        
     Where ContactId=@ContactId    
END    