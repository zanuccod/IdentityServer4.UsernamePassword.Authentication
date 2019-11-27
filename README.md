# IdentityServer4.UsernamePassword.Authentication

An example to use Identity Server 4 library for the authentication of the users.
In this example users informations are retrieved from an external resource like a database.

The program is composed from a WebApi service, an IdentityService and a console Client.

The scenario is that the client try to get resource from the WebApi service but this one allow the resources only 
to authorized user.

The authorization is provided from Identity Server.

The client requests a valid authorization token from the Identity Server 
that provides it after the credentials have been verified (username and password).

The Web Api receive now request from the client with the authorization token and verify again it with the Identity Server.
