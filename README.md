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
