create procedure [dbo].[sp_DeleteContact]    
(    
    @ContactId int     
)    
As    
BEGIN    
    DELETE FROM Contacts WHERE ContactId=@ContactId    
END   