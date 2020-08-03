create procedure [dbo].[sp_GetContactById](@ContactId int)    
AS    
BEGIN    
     SELECT * FROM Contacts as c
	 LEFT JOIN Phones as p
	 ON c.ContactId = p.ContactId
	 where c.ContactId=@ContactId    
END    