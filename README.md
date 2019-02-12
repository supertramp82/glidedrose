# glidedrose

- using JSON format for data communication as it is most wide accepted format currently - easy to integrate with any Javascript code / library / framework

- how do we know if the user authenticated? By checking for Authorization token and checking for its validity
  to get Autorization token, user needs to be authenticated in some way - HTTPS connection needs to be used because username and passwords are transferred via wire.

- code uses Jason Web Token (JWT) authentication - that token should be received from the authentication server during initial request. Token can have lifetime duration but, that's obviously doesn't make much sense, so, tokens are usually live much shorter period of time - 1, 10, 15, etc... minutes

- to avoid constantly requesting for a token, it's usually saved somewhere between api requests - can be session, or local storage, global variable, etc...

- a new token should be created if the old token expired. That usually means resending username/password repeatedly to get a new token - that means we have to store/get username/password somewhere. We can also have sliding duratioon for a token where token's expiration time is extended after each api request.

- is it always possible to buy an item? Based on offered item description

class Item {
public string Name { get; set; }
public string Description { get; set; }
public int Price { get; set; }
}

there is no way to determine if there are enough items available. So, we defintely needed to add a new field, something like

...
public int Quantity { get; set; }
...

that should be decreased every time an item purchased.

An api call should probably return an error when an item requestd bur there is not enough in the inventory

- some Item class fields changes are needed - probably, an ID should be added to the Item class, as ID will be always a unique number comparing to any other field that (Name) that could be used for item location (Name field can be duplicate at some point and it could lead to incorrect item location). Also, Price field should be a Float / Double instead of Int as it's rare to have all prices as a whole numbers.

- so far, canno make unit test for buying an item to work when using authorization. I know the reason, why it's not working - basically, there is no actual httpcontext when running unit tests, I tried to fake it but authorization still is denied.

- everything works just fine when I test "buying an item" (http://localhost:58628/api/buyitem and POST method) with authentication token (you can get an aquthentication token when you run http://localhost:58628/api/login and then add that token to request header)

- here is a sample of the Request:

POST /api/buyitem HTTP/1.1
Host: localhost:58628
Content-Type: application/json
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImdsaWRlZHJvc2UiLCJuYmYiOjE1NDk5MjQ1NTMsImV4cCI6MTU1MDUyOTM1MywiaWF0IjoxNTQ5OTI0NTUzLCJpc3MiOiJnbGlkZWRyb3NlIiwiYXVkIjoiZ2xpZGVkcm9zZSJ9.uuGDlsrwWs49DkGbZlVM8FYXoBxn6sxA1j84uGfOEU8
cache-control: no-cache
Postman-Token: 94a8f054-6299-427a-9b30-00953d06e1e6
id=4undefined=undefined

- here is a sample of response:

{"Id":4,"Name":"Zoom Pegasus 35 Turbo","Description":"Lightweight running shoe with most responsive cushioning to date","Price":180}

- this particular version of the API authentication uses hard-coded "glidedrose" username - in real life we will need to make a small modification adding authentication part and allow to create token only if the authentication is went through
