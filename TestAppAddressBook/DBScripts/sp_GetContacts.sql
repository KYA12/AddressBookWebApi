create procedure [dbo].[sp_GetContacts]    
AS    
BEGIN    
     SELECT * FROM dbo.Contacts
	 LEFT JOIN dbo.Phones
	 ON dbo.Contacts.ContactId = dbo.Phones.ContactId
END    