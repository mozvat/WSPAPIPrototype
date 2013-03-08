WSPAPIPrototype
===============

Web Services 2.0 prototype work.

Mercury Developer API Redesign
A RESTful revision


Bill Sempf
Products Of Innovative New Technology

Defining a developer-friendly API
Providing an API infrastructure that is approachable by developers of mobile applications is key to success in today’s environment. The expectations of those developers has changed significantly from what client/server developers expect. APIs are expected to be discoverable, consistent and secure by default. To this end, some standards have emerged.
REpresentational State Transfer, or REST, has become the standard for communications on the latest platforms.
JavaScript Object Notation, or JSON has replaced XML as a data stream format of choice for mobile developers.
OAuth is not the only way to authenticate users, but it has become the usual way to assure that the client that is calling a service has permission to do so.
Given all of these facts, the goal of this is to create RESTful services that provide results using JSON, and authenticate with a 2-legged OAuth scheme.  These three foci will meet the first three of the six goals of the project; the last three will be met at the implementation level.
Discoverable URL and attribute Naming
RESTful JSON based WCF architecture
Secure authentication and authorization
Business-centric metrics
Logging and tracking
Design of open source APIs
Sample services are at http://mercuryrestapi.azurewebsites.net 
RESTful nomenclature
REST, at its core, is the use of HTTP verbs to provide stateless functionality within a defined scope. The primary characteristic of a good REST API design is careful consideration of the nouns of the language, and then fitting functionality around the core HTTP verbs of PUT, GET, POST, and DELETE.
For instance, Transactions are a key part of the card services nomenclature. Transactions need to be added, changed, deleted and retrieved, or authenticated, if considered in their own data node.

POST
GET
PUT
DELETE
TransactionS
Add a new transaction
List transactions (perhaps based on a qualifier)
Batch update transactions
Remove all transactions (perhaps based on a qualifier)

Items in the transactions collection can be referred to using further nouns in the URL. The transaction identifier would be appended to deal with just one transaction.

POST
GET
PUT
DELETE
TransactionS/ABC123
Error
Get one transaction detail
Update one transaction
Delete one transaction

Queries, or GETs, can get more sophisticated. If the specification calls for all of today’s transactions, then the noun string could supply that. However, if the specification calls for all of today’s transactions that have been cleared in the state of Florida for a given vendor, then the URI will get rather unusable.
To handle that situation, the querystring is the preferred syntax. Queryable fields form the JSON data format are simply appended to the end of the URL after a question mark, fo9llowed by their respective query value.
Transactions?date=today&cleared=true&state=FL&vendor=5295147128
This gives the developer the utmost in flexibility without requiring ‘magic positions’ in the noun strings. The query values can be in any order, and a call to a query variable that isn’t queryable simply results in a 404 error that specifies the non-queryable value.

JSON Serialized Data
JavaScript Object Notation is a text based standard for the exchange of hierarchal data, similar to XML. It is based on the format the JavaScript uses to serialize objects and arrays, and is considered much more human and machine friendly than XML.
Using JSON rather than XML is fairly straightforward. Most major languages support LSON natively now, and the conversion from one to ahother is fairly simple.  A Batch Close Confirmation looks like this in XML:
<RStream>
<CmdResponse>
<ResponseOrigin>Processor</ResponseOrigin>
<DSIXReturnCode>000000</DSIXReturnCode>
<CmdStatus>Success</CmdStatus>
<TextResponse>OK TEST</TextResponse>
<UserTraceData></UserTraceData>
</CmdResponse>
<BatchClose>
<MerchantID>395347308</MerchantID>
<OperatorID>test</OperatorID>
<BatchNo>1225</BatchNo>
<BatchItemCount>18</BatchItemCount>
<NetBatchTotal>16.79</NetBatchTotal>
<CreditPurchaseCount>6</CreditPurchaseCount>
<CreditPurchaseAmount>42.25</CreditPurchaseAmount>
<CreditReturnCount>5</CreditReturnCount>
<CreditReturnAmount>31.54</CreditReturnAmount>
<DebitPurchaseCount>5</DebitPurchaseCount>
<DebitPurchaseAmount>10.17</DebitPurchaseAmount>
<DebitReturnCount>2</DebitReturnCount>
<DebitReturnAmount>4.09</DebitReturnAmount>
<ControlNo>033204606 </ControlNo>
</BatchClose>
</RStream>
And this in JSON:
{
  "RStream": {
    "CmdResponse": {
      "ResponseOrigin": "Processor",
      "DSIXReturnCode": "000000",
      "CmdStatus": "Success",
      "TextResponse": "OK TEST"
    },
    "BatchClose": {
      "MerchantID": "395347308",
      "OperatorID": "test",
      "BatchNo": "1225",
      "BatchItemCount": "18",
      "NetBatchTotal": "16.79",
      "CreditPurchaseCount": "6",
      "CreditPurchaseAmount": "42.25",
      "CreditReturnCount": "5",
      "CreditReturnAmount": "31.54",
      "DebitPurchaseCount": "5",
      "DebitPurchaseAmount": "10.17",
      "DebitReturnCount": "2",
      "DebitReturnAmount": "4.09",
      "ControlNo": "033204606 "
    }
  }
}

Authenticated Sessions
To assure authenticity of requests, all interactions with any service will include a hash-based message authentication code using HMAC-SHA1. This technique, part of the OAuth standard and used by Amazon, is becoming the de facto standard for authenticating the viability of requests.

The figure shows the flow from the client to the service.  The client first gathers the data for the actual service call, as specified by the JSON data format descriptions below. Metadata is then added in the form of their public ‘customer identifier’, and a datestamp in UNC time. Their secret is them used to hash the data together using the HMAC-SHA1 format. Finally, the data, metadata and hash are all sent to the server over a TLS channel.
When the server receives the data, the first operation is to verify that the timestamp is within the last 5 minutes. If it is, then the client’s public ‘customer identifier’ is used to get their secret from the database. The encoded URI is hashed using the exact same process as the client used, and the hashes are compared. If they match, the request is processed.
Payments
Rest Nomenclature
We just have batches and transactions here. It seems like card type is more of a property than anything else. Transactions are the preauth for a card interaction.  Batches are the finalization of a set of transactions.
Gift Cards are rolled into payments, and the AccountType enumerator has been added to handle the business logic on the back end.
By necessity, there is some simplification of the system, but that is acceptable for the REST API. Use cases for the REST API are for simple scenarios. More complex scenarios will use the SOAP or direct communication methods.


Post
Get
Put
Delete
Transactions
CreditTransaction
CTranDetail with a transaction ID or CAllDetail without
Error
Reversal
Batches
Set up a batch
CBatch
Update a batch
Error

Usage
Transactions are committed on card swipe (or entry). A POST operation should return an auth packet with the original data.
Transactions
JSON
{
   "TransactionId":0,
   "OperatorId":0,
   "TranType":null,
   "TranCode":null,
   "InvoiceNo":0,
   "RefNo":0,
   "Account":null,
   "AccountType":null,
   "Amount":null
}
POST
POST http://localhost:49535/Api/Payments/Transactions?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a HTTP/1.1
User-Agent: Fiddler
Host: localhost:49535
Content-Type: application/json; charset=utf-8
Content-Length: 193

   {
      "TransactionId":0,
      "OperatorId":0,
      "TranType":null,
      "TranCode":null,
      "InvoiceNo":0,
      "RefNo":0,
      "Account":null,
      "Amount":null
   }

HTTP/1.1 201 Created
Cache-Control: no-cache
Pragma: no-cache
Content-Type: application/json; charset=utf-8
Expires: -1
Location: http://localhost:49535/api/Transactions/0
Server: Microsoft-IIS/8.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?RDpcQ29kZVxzZW1wZl9tZXJjdXJ5cmVzdGFwaVxNZXJjdXJ5UmVzdEFwaVxBcGlcUGF5bWVudHNcVHJhbnNhY3Rpb25z?=
X-Powered-By: ASP.NET
Date: Tue, 26 Feb 2013 21:42:50 GMT
Content-Length: 119
   {
      "TransactionId":0,
      "OperatorId":0,
      "TranType":null,
      "TranCode":null,
      "InvoiceNo":0,
      "RefNo":0,
      "Account":null,
      "Amount":null
   }

GET
GET http://localhost:49535/Api/Payments/Transactions?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a HTTP/1.1
User-Agent: Fiddler
Host: localhost:49535
HTTP/1.1 200 OK
Cache-Control: no-cache
Pragma: no-cache
Content-Type: application/json; charset=utf-8
Expires: -1
Server: Microsoft-IIS/8.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?RDpcQ29kZVxzZW1wZl9tZXJjdXJ5cmVzdGFwaVxNZXJjdXJ5UmVzdEFwaVxBcGlcUGF5bWVudHNcVHJhbnNhY3Rpb25z?=
X-Powered-By: ASP.NET
Date: Tue, 26 Feb 2013 21:35:33 GMT
Content-Length: 241
[
   {
      "TransactionId":0,
      "OperatorId":0,
      "TranType":null,
      "TranCode":null,
      "InvoiceNo":0,
      "RefNo":0,
      "Account":null,
      "Amount":null
   },
   {
      "TransactionId":0,
      "OperatorId":0,
      "TranType":null,
      "TranCode":null,
      "InvoiceNo":0,
      "RefNo":0,
      "Account":null,
      "Amount":null
   }
]
PUT
PUT http://localhost:49535/Api/Payments/Transactions?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a&id=1 HTTP/1.1
User-Agent: Fiddler
Host: localhost:49535
Content-Type: application/json; charset=utf-8
Content-Length: 193

   {
      "TransactionId":0,
      "OperatorId":0,
      "TranType":null,
      "TranCode":null,
      "InvoiceNo":0,
      "RefNo":0,
      "Account":null,
      "Amount":null
   }
HTTP/1.1 204 No Content
Cache-Control: no-cache
Pragma: no-cache
Content-Type: text/html; charset=utf-8
Expires: -1
Server: Microsoft-IIS/8.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?RDpcQ29kZVxzZW1wZl9tZXJjdXJ5cmVzdGFwaVxNZXJjdXJ5UmVzdEFwaVxBcGlcUGF5bWVudHNcVHJhbnNhY3Rpb25z?=
X-Powered-By: ASP.NET
Date: Tue, 26 Feb 2013 21:45:01 GMT
DELETE
DELETE http://localhost:49535/Api/Payments/Transactions?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a&id=1 HTTP/1.1
User-Agent: Fiddler
Host: localhost:49535
Content-Type: application/json; charset=utf-8
Content-Length: 0

HTTP/1.1 204 No Content
Cache-Control: no-cache
Pragma: no-cache
Content-Type: text/html; charset=utf-8
Expires: -1
Server: Microsoft-IIS/8.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?RDpcQ29kZVxzZW1wZl9tZXJjdXJ5cmVzdGFwaVxNZXJjdXJ5UmVzdEFwaVxBcGlcUGF5bWVudHNcVHJhbnNhY3Rpb25z?=
X-Powered-By: ASP.NET
Date: Tue, 26 Feb 2013 21:45:40 GMT
Batches
 JSON
   {
      "IpAddress":null,
      "IpPort":null,
      "MerchantId":0,
      "TerminalId":0,
      "OperatorId":0,
      "TranCode":null,
      "SequenceNo":0,
      "TerminalName":null,
      "ShiftId":0,
      "Signature":null
   }
POST
POST http://localhost:49535/Api/Payments/Batches?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a HTTP/1.1
User-Agent: Fiddler
Host: localhost:49535
Content-Type: application/json; charset=utf-8
Content-Length: 247

   {
      "IpAddress":null,
      "IpPort":null,
      "MerchantId":0,
      "TerminalId":0,
      "OperatorId":0,
      "TranCode":null,
      "SequenceNo":0,
      "TerminalName":null,
      "ShiftId":0,
      "Signature":null
   }
HTTP/1.1 204 No Content
Cache-Control: no-cache
Pragma: no-cache
Content-Type: text/html; charset=utf-8
Expires: -1
Server: Microsoft-IIS/8.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?RDpcQ29kZVxzZW1wZl9tZXJjdXJ5cmVzdGFwaVxNZXJjdXJ5UmVzdEFwaVxBcGlcUGF5bWVudHNcQmF0Y2hlcw==?=
X-Powered-By: ASP.NET
Date: Tue, 26 Feb 2013 21:49:52 GMT
GET
GET http://localhost:49535/Api/Payments/Batches?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a HTTP/1.1
User-Agent: Fiddler
Host: localhost:49535

HTTP/1.1 200 OK
Cache-Control: no-cache
Pragma: no-cache
Content-Type: application/json; charset=utf-8
Expires: -1
Server: Microsoft-IIS/8.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?RDpcQ29kZVxzZW1wZl9tZXJjdXJ5cmVzdGFwaVxNZXJjdXJ5UmVzdEFwaVxBcGlcUGF5bWVudHNcQmF0Y2hlcw==?=
X-Powered-By: ASP.NET
Date: Tue, 26 Feb 2013 21:47:20 GMT
Content-Length: 317
[
   {
      "IpAddress":null,
      "IpPort":null,
      "MerchantId":0,
      "TerminalId":0,
      "OperatorId":0,
      "TranCode":null,
      "SequenceNo":0,
      "TerminalName":null,
      "ShiftId":0,
      "Signature":null
   },
   {
      "IpAddress":null,
      "IpPort":null,
      "MerchantId":0,
      "TerminalId":0,
      "OperatorId":0,
      "TranCode":null,
      "SequenceNo":0,
      "TerminalName":null,
      "ShiftId":0,
      "Signature":null
   }
]
PUT
PUT http://localhost:49535/Api/Payments/Batches?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a?id=1 HTTP/1.1
User-Agent: Fiddler
Host: localhost:49535
Content-Type: application/json; charset=utf-8
Content-Length: 247

   {
      "IpAddress":null,
      "IpPort":null,
      "MerchantId":0,
      "TerminalId":0,
      "OperatorId":0,
      "TranCode":null,
      "SequenceNo":0,
      "TerminalName":null,
      "ShiftId":0,
      "Signature":null
   }

HTTP/1.1 201 Created
Cache-Control: no-cache
Pragma: no-cache
Content-Type: application/json; charset=utf-8
Expires: -1
Location: http://localhost:49535/api/Batches/0
Server: Microsoft-IIS/8.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?RDpcQ29kZVxzZW1wZl9tZXJjdXJ5cmVzdGFwaVxNZXJjdXJ5UmVzdEFwaVxBcGlcUGF5bWVudHNcQmF0Y2hlcw==?=
X-Powered-By: ASP.NET
Date: Tue, 26 Feb 2013 21:50:41 GMT
Content-Length: 157
   {
      "IpAddress":null,
      "IpPort":null,
      "MerchantId":0,
      "TerminalId":0,
      "OperatorId":0,
      "TranCode":null,
      "SequenceNo":0,
      "TerminalName":null,
      "ShiftId":0,
      "Signature":null
   }

DELETE
DELETE http://localhost:49535/Api/Payments/Batches?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a&id=1 HTTP/1.1
User-Agent: Fiddler
Host: localhost:49535
Content-Type: application/json; charset=utf-8
Content-Length: 0

HTTP/1.1 204 No Content
Cache-Control: no-cache
Pragma: no-cache
Content-Type: text/html; charset=utf-8
Expires: -1
Server: Microsoft-IIS/8.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?RDpcQ29kZVxzZW1wZl9tZXJjdXJ5cmVzdGFwaVxNZXJjdXJ5UmVzdEFwaVxBcGlcUGF5bWVudHNcQmF0Y2hlcw==?=
X-Powered-By: ASP.NET
Date: Tue, 26 Feb 2013 21:51:26 GMT


Hosted Checkout
Rest Nomenclature


Post
Get
Put
Delete
Cards
InitCardInfo
VerifyCardInfo
Error
Error
Payments
InitPayment
VerifyPaymentInfo
Error
Error

Usage

Cards
JSON
   {
      "MerchantId":0,
      "Password":null,
      "Frequency":null,
      "CardHolderName":null,
      "OperatorId":null,
      "Keypad":null,
      "DefaultSwipe":null,
      "CardEntryMethod":null,
      "ProcessCompleteUrl":null,
      "ReturnUrl":null,
      "PageTimeoutDuration":null,
      "PageProperties":null
   }

POST
POST http://localhost:49535/api/HostedCheckout/Cards?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a HTTP/1.1
User-Agent: Fiddler
Host: localhost:49535
Content-Type: application/json; charset=utf-8
Content-Length: 344

   {
      "MerchantId":0,
      "Password":null,
      "Frequency":null,
      "CardHolderName":null,
      "OperatorId":null,
      "Keypad":null,
      "DefaultSwipe":null,
      "CardEntryMethod":null,
      "ProcessCompleteUrl":null,
      "ReturnUrl":null,
      "PageTimeoutDuration":null,
      "PageProperties":null
   }

HTTP/1.1 200 OK
Cache-Control: no-cache
Pragma: no-cache
Content-Type: application/json; charset=utf-8
Expires: -1
Server: Microsoft-IIS/8.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?RDpcQ29kZVxzZW1wZl9tZXJjdXJ5cmVzdGFwaVxNZXJjdXJ5UmVzdEFwaVxhcGlcSG9zdGVkQ2hlY2tvdXRcQ2FyZHM=?=
X-Powered-By: ASP.NET
Date: Thu, 28 Feb 2013 21:20:51 GMT
Content-Length: 47

{"ResponseCode":0,"CardId":null,"Message":null}
GET
GET http://localhost:49535/api/HostedCheckout/Cards?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a HTTP/1.1
User-Agent: Fiddler
Host: localhost:49535
HTTP/1.1 200 OK
Cache-Control: no-cache
Pragma: no-cache
Content-Type: application/json; charset=utf-8
Expires: -1
Server: Microsoft-IIS/8.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?RDpcQ29kZVxzZW1wZl9tZXJjdXJ5cmVzdGFwaVxNZXJjdXJ5UmVzdEFwaVxhcGlcSG9zdGVkQ2hlY2tvdXRcQ2FyZHM=?=
X-Powered-By: ASP.NET
Date: Thu, 28 Feb 2013 19:43:40 GMT
Content-Length: 479
[
   {
      "MerchantId":0,
      "Password":null,
      "Frequency":null,
      "CardHolderName":null,
      "OperatorId":null,
      "Keypad":null,
      "DefaultSwipe":null,
      "CardEntryMethod":null,
      "ProcessCompleteUrl":null,
      "ReturnUrl":null,
      "PageTimeoutDuration":null,
      "PageProperties":null
   },
   {
      "MerchantId":0,
      "Password":null,
      "Frequency":null,
      "CardHolderName":null,
      "OperatorId":null,
      "Keypad":null,
      "DefaultSwipe":null,
      "CardEntryMethod":null,
      "ProcessCompleteUrl":null,
      "ReturnUrl":null,
      "PageTimeoutDuration":null,
      "PageProperties":null
   }
]
Payments
JSON
   {
      "MerchantId":0,
      "Password":null,
      "TranType":null,
      "TotalAmount":0.0,
      "PartialAuth":null,
      "Frequency":null,
      "VoiceAuthCode":null,
      "OperatorId":null,
      "TerminalName":null,
      "Invoice":null,
      "Memo":null,
      "TaxAmount":0.0,
      "AVSFields":null,
      "AVSAddress":null,
      "AVSZip":null,
      "CustomerCode":null,
      "Keypad":null,
      "DefaultSwipe":null,
      "CardEntryMethod":null,
      "PageProperities":null,
      "OrderTotal":null,
      "JCB":null,
      "Diners":null
   }

POST
POST http://localhost:49535/api/HostedCheckout/Payments?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a HTTP/1.1
User-Agent: Fiddler
Host: localhost:49535
Content-Type: application/json; charset=utf-8
Content-Length: 590

   {
      "MerchantId":0,
      "Password":null,
      "TranType":null,
      "TotalAmount":0.0,
      "PartialAuth":null,
      "Frequency":null,
      "VoiceAuthCode":null,
      "OperatorId":null,
      "TerminalName":null,
      "Invoice":null,
      "Memo":null,
      "TaxAmount":0.0,
      "AVSFields":null,
      "AVSAddress":null,
      "AVSZip":null,
      "CustomerCode":null,
      "Keypad":null,
      "DefaultSwipe":null,
      "CardEntryMethod":null,
      "PageProperities":null,
      "OrderTotal":null,
      "JCB":null,
      "Diners":null
   }

HTTP/1.1 200 OK
Cache-Control: no-cache
Pragma: no-cache
Content-Type: application/json; charset=utf-8
Expires: -1
Server: Microsoft-IIS/8.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?RDpcQ29kZVxzZW1wZl9tZXJjdXJ5cmVzdGFwaVxNZXJjdXJ5UmVzdEFwaVxhcGlcSG9zdGVkQ2hlY2tvdXRcUGF5bWVudHM=?=
X-Powered-By: ASP.NET
Date: Thu, 28 Feb 2013 21:23:42 GMT
Content-Length: 502
{
   "ResponseCode":0,
   "PaymentId":null,
   "Status":null,
   "StatusMessage":null,
   "AcqRefData":null,
   "Amount":0.0,
   "AuthCode":null,
   "AuthAmount":0.0,
   "PurchaseAmount":0.0,
   "AVSAddress":null,
   "AVSResult":null,
   "AVSZip":null,
   "CardholderName":null,
   "CardType":null,
   "CustomerCode":null,
   "CVVResult":null,
   "DisplayMessage":null,
   "ExpDate":null,
   "Invoice":null,
   "MaskAccount":null,
   "Memo":null,
   "PaymentIdExpired":false,
   "RefNo":null,
   "TaxAmount":0.0,
   "Token":null,
   "TransDateTime":"0001-01-01T00:00:00",
   "TranType":null,
   "ProcessData":null
}
GET
GET http://localhost:49535/api/HostedCheckout/Payments?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a HTTP/1.1
User-Agent: Fiddler
Host: localhost:49535
Content-Type: application/json; charset=utf-8
Content-Length: 0
HTTP/1.1 200 OK
Cache-Control: no-cache
Pragma: no-cache
Content-Type: application/json; charset=utf-8
Expires: -1
Server: Microsoft-IIS/8.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?RDpcQ29kZVxzZW1wZl9tZXJjdXJ5cmVzdGFwaVxNZXJjdXJ5UmVzdEFwaVxhcGlcSG9zdGVkQ2hlY2tvdXRcUGF5bWVudHM=?=
X-Powered-By: ASP.NET
Date: Thu, 28 Feb 2013 21:21:54 GMT
Content-Length: 795
[
   {
      "MerchantId":0,
      "Password":null,
      "TranType":null,
      "TotalAmount":0.0,
      "PartialAuth":null,
      "Frequency":null,
      "VoiceAuthCode":null,
      "OperatorId":null,
      "TerminalName":null,
      "Invoice":null,
      "Memo":null,
      "TaxAmount":0.0,
      "AVSFields":null,
      "AVSAddress":null,
      "AVSZip":null,
      "CustomerCode":null,
      "Keypad":null,
      "DefaultSwipe":null,
      "CardEntryMethod":null,
      "PageProperities":null,
      "OrderTotal":null,
      "JCB":null,
      "Diners":null
   },
   {
      "MerchantId":0,
      "Password":null,
      "TranType":null,
      "TotalAmount":0.0,
      "PartialAuth":null,
      "Frequency":null,
      "VoiceAuthCode":null,
      "OperatorId":null,
      "TerminalName":null,
      "Invoice":null,
      "Memo":null,
      "TaxAmount":0.0,
      "AVSFields":null,
      "AVSAddress":null,
      "AVSZip":null,
      "CustomerCode":null,
      "Keypad":null,
      "DefaultSwipe":null,
      "CardEntryMethod":null,
      "PageProperities":null,
      "OrderTotal":null,
      "JCB":null,
      "Diners":null
   }
]


Loyalty

REST nomenclature

Post
Get
Put
Delete
Credits
AddCredits
InquireCredits
DebitCredits / CloseTransaction / CloseTransaction
WithSku
CancelTransaction
COUPONS
SaveCoupon
Validate a coupon?
RedeemCoupon
Delete a coupon? Expire it?
PROGRAMS
Add a program
GetProgramDescription
Update a program
Disable a program?
CustomData
StoreCustomData
GetCustomData
Update custom data
Delete custom data

Usage
Credits
JSON
{
"CustomerIdentifier"
"Units"
"Description"
"Employee_id"
"Station_Id"
"Ticket_Id"
"Revenue"
"CardNumber"
"Language"
"Version"
"Delay"
"Testmode"
"Match"
"DeviceId"
}
POST
POST http://localhost:49535/api/loyalty/credits?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a HTTP/1.1
User-Agent: Fiddler
Host: localhost:49535
Content-Type: application/json; charset=utf-8
Content-Length: 320

   {
      "CustomerIdentifier":null,
      "Description":null,
      "EmployeeID":0,
      "StationId":0,
      "TicketId":0,
      "Revenue":0.0,
      "CardNumber":null,
      "Language":null,
      "Version":null,
      "Delay":null,
      "TestMode":null,
      "Match":null,
      "DeviceId":0
   }

HTTP/1.1 201 Created
Cache-Control: no-cache
Pragma: no-cache
Content-Type: application/json; charset=utf-8
Expires: -1
Location: http://localhost:49535/api/credits/0
Server: Microsoft-IIS/8.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?RDpcQ29kZVxzZW1wZl9tZXJjdXJ5cmVzdGFwaVxNZXJjdXJ5UmVzdEFwaVxhcGlcbG95YWx0eVxjcmVkaXRz?=
X-Powered-By: ASP.NET
Date: Fri, 01 Mar 2013 23:21:24 GMT
Content-Length: 206
   {
      "CustomerIdentifier":null,
      "Description":null,
      "EmployeeID":0,
      "StationId":0,
      "TicketId":0,
      "Revenue":0.0,
      "CardNumber":null,
      "Language":null,
      "Version":null,
      "Delay":null,
      "TestMode":null,
      "Match":null,
      "DeviceId":0
   }

GET
GET http://localhost:49535/api/loyalty/credits?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a HTTP/1.1
User-Agent: Fiddler
Host: localhost:49535

HTTP/1.1 200 OK
Cache-Control: no-cache
Pragma: no-cache
Content-Type: application/json; charset=utf-8
Expires: -1
Server: Microsoft-IIS/8.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?RDpcQ29kZVxzZW1wZl9tZXJjdXJ5cmVzdGFwaVxNZXJjdXJ5UmVzdEFwaVxhcGlcbG95YWx0eVxjcmVkaXRz?=
X-Powered-By: ASP.NET
Date: Fri, 01 Mar 2013 21:17:02 GMT
Content-Length: 415
[
   {
      "CustomerIdentifier":null,
      "Description":null,
      "EmployeeID":0,
      "StationId":0,
      "TicketId":0,
      "Revenue":0.0,
      "CardNumber":null,
      "Language":null,
      "Version":null,
      "Delay":null,
      "TestMode":null,
      "Match":null,
      "DeviceId":0
   },
   {
      "CustomerIdentifier":null,
      "Description":null,
      "EmployeeID":0,
      "StationId":0,
      "TicketId":0,
      "Revenue":0.0,
      "CardNumber":null,
      "Language":null,
      "Version":null,
      "Delay":null,
      "TestMode":null,
      "Match":null,
      "DeviceId":0
   }
]
PUT
PUT http://localhost:49535/api/loyalty/credits?id=1001&customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a HTTP/1.1
User-Agent: Fiddler
Host: localhost:49535
Content-Type: application/json; charset=utf-8
Content-Length: 320

   {
      "CustomerIdentifier":null,
      "Description":null,
      "EmployeeID":0,
      "StationId":0,
      "TicketId":0,
      "Revenue":0.0,
      "CardNumber":null,
      "Language":null,
      "Version":null,
      "Delay":null,
      "TestMode":null,
      "Match":null,
      "DeviceId":0
   }

HTTP/1.1 204 No Content
Cache-Control: no-cache
Pragma: no-cache
Expires: -1
Server: Microsoft-IIS/8.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?RDpcQ29kZVxzZW1wZl9tZXJjdXJ5cmVzdGFwaVxNZXJjdXJ5UmVzdEFwaVxhcGlcbG95YWx0eVxjcmVkaXRz?=
X-Powered-By: ASP.NET
Date: Fri, 01 Mar 2013 23:22:40 GMT



DELETE
DELETE http://localhost:49535/api/loyalty/credits?id=1001&customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a HTTP/1.1
User-Agent: Fiddler
Host: localhost:49535
Content-Type: application/json; charset=utf-8
Content-Length: 0


HTTP/1.1 204 No Content
Cache-Control: no-cache
Pragma: no-cache
Expires: -1
Server: Microsoft-IIS/8.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?RDpcQ29kZVxzZW1wZl9tZXJjdXJ5cmVzdGFwaVxNZXJjdXJ5UmVzdEFwaVxhcGlcbG95YWx0eVxjcmVkaXRz?=
X-Powered-By: ASP.NET
Date: Fri, 01 Mar 2013 23:23:13 GMT


Coupons
JSON
   {
      "Code":null,
      "CouponClass":0,
      "CouponType":0,
      "CreateDate":"0001-01-01T00:00:00",
      "Customer":null,
      "Description":null,
      "Disclaimer":null,
      "Discount":0.0,
      "ExpireDate":"0001-01-01T00:00:00",
      "QuantityDiscounted":0.0,
      "RedeemDate":"0001-01-01T00:00:00",
      "SkuBasket":null,
      "SkuQuantityRequired":0.0,
      "Source":null,
      "Title":null
   }

POST
POST http://mercuryrestapi.azurewebsites.net/api/coupons?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a HTTP/1.1
User-Agent: Fiddler
Host: mercuryrestapi.azurewebsites.net
Content-Length: 443
Content-Type: Text/Json

   {
      "Code":null,
      "CouponClass":0,
      "CouponType":0,
      "CreateDate":"0001-01-01T00:00:00",
      "Customer":null,
      "Description":null,
      "Disclaimer":null,
      "Discount":0.0,
      "ExpireDate":"0001-01-01T00:00:00",
      "QuantityDiscounted":0.0,
      "RedeemDate":"0001-01-01T00:00:00",
      "SkuBasket":null,
      "SkuQuantityRequired":0.0,
      "Source":null,
      "Title":null
   }

HTTP/1.1 201 Created
Cache-Control: no-cache
Pragma: no-cache
Content-Length: 312
Content-Type: text/json; charset=utf-8
Expires: -1
Location: http://mercuryrestapi.azurewebsites.net/api/coupons
Server: Microsoft-IIS/7.5
Set-Cookie: ARRAffinity=74f778652d34ffe1759f3fb7e01fca52904d95872e7225cb1a6098144cce9c96;Path=/;Domain=mercuryrestapi.azurewebsites.net
X-AspNet-Version: 4.0.30319
X-Powered-By: ASP.NET
X-Powered-By: ARR/2.5
X-Powered-By: ASP.NET
Date: Fri, 22 Feb 2013 21:33:09 GMT

  {
      "Code":null,
      "CouponClass":0,
      "CouponType":0,
      "CreateDate":"0001-01-01T00:00:00",
      "Customer":null,
      "Description":null,
      "Disclaimer":null,
      "Discount":0.0,
      "ExpireDate":"0001-01-01T00:00:00",
      "QuantityDiscounted":0.0,
      "RedeemDate":"0001-01-01T00:00:00",
      "SkuBasket":null,
      "SkuQuantityRequired":0.0,
      "Source":null,
      "Title":null
   }

GET
GET http://mercuryrestapi.azurewebsites.net/api/coupons?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a HTTP/1.1
User-Agent: Fiddler
Host: mercuryrestapi.azurewebsites.net

HTTP/1.1 200 OK
Cache-Control: no-cache
Pragma: no-cache
Content-Length: 627
Content-Type: application/json; charset=utf-8
Expires: -1
Server: Microsoft-IIS/7.5
Set-Cookie: ARRAffinity=74f778652d34ffe1759f3fb7e01fca52904d95872e7225cb1a6098144cce9c96;Path=/;Domain=mercuryrestapi.azurewebsites.net
X-AspNet-Version: 4.0.30319
X-Powered-By: ASP.NET
X-Powered-By: ARR/2.5
X-Powered-By: ASP.NET
Date: Fri, 22 Feb 2013 20:38:28 GMT
[
   {
      "Code":null,
      "CouponClass":0,
      "CouponType":0,
      "CreateDate":"0001-01-01T00:00:00",
      "Customer":null,
      "Description":null,
      "Disclaimer":null,
      "Discount":0.0,
      "ExpireDate":"0001-01-01T00:00:00",
      "QuantityDiscounted":0.0,
      "RedeemDate":"0001-01-01T00:00:00",
      "SkuBasket":null,
      "SkuQuantityRequired":0.0,
      "Source":null,
      "Title":null
   },
   {
      "Code":null,
      "CouponClass":0,
      "CouponType":0,
      "CreateDate":"0001-01-01T00:00:00",
      "Customer":null,
      "Description":null,
      "Disclaimer":null,
      "Discount":0.0,
      "ExpireDate":"0001-01-01T00:00:00",
      "QuantityDiscounted":0.0,
      "RedeemDate":"0001-01-01T00:00:00",
      "SkuBasket":null,
      "SkuQuantityRequired":0.0,
      "Source":null,
      "Title":null
   }
]
PUT
PUT http://mercuryrestapi.azurewebsites.net/api/coupons?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a&id=1001 HTTP/1.1
User-Agent: Fiddler
Host: mercuryrestapi.azurewebsites.net
Content-Length: 443
Content-Type: Text/Json

   {
      "Code":null,
      "CouponClass":0,
      "CouponType":0,
      "CreateDate":"0001-01-01T00:00:00",
      "Customer":null,
      "Description":null,
      "Disclaimer":null,
      "Discount":0.0,
      "ExpireDate":"0001-01-01T00:00:00",
      "QuantityDiscounted":0.0,
      "RedeemDate":"0001-01-01T00:00:00",
      "SkuBasket":null,
      "SkuQuantityRequired":0.0,
      "Source":null,
      "Title":null
   }

HTTP/1.1 204 No Content
Cache-Control: no-cache
Pragma: no-cache
Expires: -1
Server: Microsoft-IIS/7.5
Set-Cookie: ARRAffinity=74f778652d34ffe1759f3fb7e01fca52904d95872e7225cb1a6098144cce9c96;Path=/;Domain=mercuryrestapi.azurewebsites.net
X-AspNet-Version: 4.0.30319
X-Powered-By: ASP.NET
X-Powered-By: ARR/2.5
X-Powered-By: ASP.NET
Date: Fri, 22 Feb 2013 21:34:22 GMT

DELETE
DELETE http://mercuryrestapi.azurewebsites.net/api/coupons?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a&id=1001 HTTP/1.1
User-Agent: Fiddler
Host: mercuryrestapi.azurewebsites.net
Content-Length: 0
Content-Type: Text/Json


HTTP/1.1 204 No Content
Cache-Control: no-cache
Pragma: no-cache
Expires: -1
Server: Microsoft-IIS/7.5
Set-Cookie: ARRAffinity=74f778652d34ffe1759f3fb7e01fca52904d95872e7225cb1a6098144cce9c96;Path=/;Domain=mercuryrestapi.azurewebsites.net
X-AspNet-Version: 4.0.30319
X-Powered-By: ASP.NET
X-Powered-By: ARR/2.5
X-Powered-By: ASP.NET
Date: Fri, 22 Feb 2013 21:35:05 GMT

Programs
JSON
   {
      "Action":null,
      "BonusForEmail":0,
      "BonusForReg":0,
      "BonusForSocialMedia":0,
      "BonusForSurvey":0,
      "BusinessEmail":null,
      "BusinessName":null,
      "BusinessUrl":null,
      "Goal":0,
      "Incentive":null,
      "KeywordId":0,
      "KeywordName":null,
      "KioskAreaCode":false,
      "LogoUrl":null,
      "Options":null,
      "RegAreaCode":null,
      "RegImg1024x768":null,
      "RegImg1280x1024":null,
      "RegImg800x600":null,
      "ShortCode":null,
      "Units":null
   }
POST
POST http://localhost:49535/api/loyalty/programs?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a HTTP/1.1
User-Agent: Fiddler
Host: localhost:49535
Content-Type: application/json; charset=utf-8
Content-Length: 556

   {
      "Action":null,
      "BonusForEmail":0,
      "BonusForReg":0,
      "BonusForSocialMedia":0,
      "BonusForSurvey":0,
      "BusinessEmail":null,
      "BusinessName":null,
      "BusinessUrl":null,
      "Goal":0,
      "Incentive":null,
      "KeywordId":0,
      "KeywordName":null,
      "KioskAreaCode":false,
      "LogoUrl":null,
      "Options":null,
      "RegAreaCode":null,
      "RegImg1024x768":null,
      "RegImg1280x1024":null,
      "RegImg800x600":null,
      "ShortCode":null,
      "Units":null
   }

HTTP/1.1 201 Created
Cache-Control: no-cache
Pragma: no-cache
Content-Type: application/json; charset=utf-8
Expires: -1
Location: http://localhost:49535/api/programs
Server: Microsoft-IIS/8.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?RDpcQ29kZVxzZW1wZl9tZXJjdXJ5cmVzdGFwaVxNZXJjdXJ5UmVzdEFwaVxhcGlcbG95YWx0eVxwcm9ncmFtcw==?=
X-Powered-By: ASP.NET
Date: Fri, 01 Mar 2013 23:25:46 GMT
Content-Length: 378

   {
      "Action":null,
      "BonusForEmail":0,
      "BonusForReg":0,
      "BonusForSocialMedia":0,
      "BonusForSurvey":0,
      "BusinessEmail":null,
      "BusinessName":null,
      "BusinessUrl":null,
      "Goal":0,
      "Incentive":null,
      "KeywordId":0,
      "KeywordName":null,
      "KioskAreaCode":false,
      "LogoUrl":null,
      "Options":null,
      "RegAreaCode":null,
      "RegImg1024x768":null,
      "RegImg1280x1024":null,
      "RegImg800x600":null,
      "ShortCode":null,
      "Units":null
   }

GET
GET http://localhost:49535/api/loyalty/programs?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a HTTP/1.1
User-Agent: Fiddler
Host: localhost:49535


HTTP/1.1 200 OK
Cache-Control: no-cache
Pragma: no-cache
Content-Type: application/json; charset=utf-8
Expires: -1
Server: Microsoft-IIS/8.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?RDpcQ29kZVxzZW1wZl9tZXJjdXJ5cmVzdGFwaVxNZXJjdXJ5UmVzdEFwaVxhcGlcbG95YWx0eVxwcm9ncmFtcw==?=
X-Powered-By: ASP.NET
Date: Fri, 01 Mar 2013 23:24:27 GMT
Content-Length: 759
[
   {
      "Action":null,
      "BonusForEmail":0,
      "BonusForReg":0,
      "BonusForSocialMedia":0,
      "BonusForSurvey":0,
      "BusinessEmail":null,
      "BusinessName":null,
      "BusinessUrl":null,
      "Goal":0,
      "Incentive":null,
      "KeywordId":0,
      "KeywordName":null,
      "KioskAreaCode":false,
      "LogoUrl":null,
      "Options":null,
      "RegAreaCode":null,
      "RegImg1024x768":null,
      "RegImg1280x1024":null,
      "RegImg800x600":null,
      "ShortCode":null,
      "Units":null
   },
   {
      "Action":null,
      "BonusForEmail":0,
      "BonusForReg":0,
      "BonusForSocialMedia":0,
      "BonusForSurvey":0,
      "BusinessEmail":null,
      "BusinessName":null,
      "BusinessUrl":null,
      "Goal":0,
      "Incentive":null,
      "KeywordId":0,
      "KeywordName":null,
      "KioskAreaCode":false,
      "LogoUrl":null,
      "Options":null,
      "RegAreaCode":null,
      "RegImg1024x768":null,
      "RegImg1280x1024":null,
      "RegImg800x600":null,
      "ShortCode":null,
      "Units":null
   }
]
PUT
PUT http://localhost:49535/api/loyalty/programs?id=1001&customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a HTTP/1.1
User-Agent: Fiddler
Host: localhost:49535
Content-Type: application/json; charset=utf-8
Content-Length: 556

   {
      "Action":null,
      "BonusForEmail":0,
      "BonusForReg":0,
      "BonusForSocialMedia":0,
      "BonusForSurvey":0,
      "BusinessEmail":null,
      "BusinessName":null,
      "BusinessUrl":null,
      "Goal":0,
      "Incentive":null,
      "KeywordId":0,
      "KeywordName":null,
      "KioskAreaCode":false,
      "LogoUrl":null,
      "Options":null,
      "RegAreaCode":null,
      "RegImg1024x768":null,
      "RegImg1280x1024":null,
      "RegImg800x600":null,
      "ShortCode":null,
      "Units":null
   }

HTTP/1.1 204 No Content
Cache-Control: no-cache
Pragma: no-cache
Expires: -1
Server: Microsoft-IIS/8.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?RDpcQ29kZVxzZW1wZl9tZXJjdXJ5cmVzdGFwaVxNZXJjdXJ5UmVzdEFwaVxhcGlcbG95YWx0eVxwcm9ncmFtcw==?=
X-Powered-By: ASP.NET
Date: Fri, 01 Mar 2013 23:27:04 GMT
 
DELETE
DELETE http://localhost:49535/api/loyalty/programs?id=1001&customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a HTTP/1.1
User-Agent: Fiddler
Host: localhost:49535
Content-Type: application/json; charset=utf-8
Content-Length: 0


HTTP/1.1 204 No Content
Cache-Control: no-cache
Pragma: no-cache
Expires: -1
Server: Microsoft-IIS/8.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?RDpcQ29kZVxzZW1wZl9tZXJjdXJ5cmVzdGFwaVxNZXJjdXJ5UmVzdEFwaVxhcGlcbG95YWx0eVxwcm9ncmFtcw==?=
X-Powered-By: ASP.NET
Date: Fri, 01 Mar 2013 23:27:33 GMT

CustomData
JSON
   {
      "CreateDate":"0001-01-01T00:00:00",
      "CustomerId":0,
      "DataGroup":null,
      "DataName":null,
      "DataType":0,
      "DataValue":null,
      "Id":0,
      "KeywordId":0,
      "LastUpdate":"0001-01-01T00:00:00"
   }
POST
POST http://localhost:49535/api/loyalty/customdata?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a HTTP/1.1
User-Agent: Fiddler
Host: localhost:49535
Content-Type: application/json; charset=utf-8
Content-Length: 556

   {
      "Action":null,
      "BonusForEmail":0,
      "BonusForReg":0,
      "BonusForSocialMedia":0,
      "BonusForSurvey":0,
      "BusinessEmail":null,
      "BusinessName":null,
      "BusinessUrl":null,
      "Goal":0,
      "Incentive":null,
      "KeywordId":0,
      "KeywordName":null,
      "KioskAreaCode":false,
      "LogoUrl":null,
      "Options":null,
      "RegAreaCode":null,
      "RegImg1024x768":null,
      "RegImg1280x1024":null,
      "RegImg800x600":null,
      "ShortCode":null,
      "Units":null
   }

HTTP/1.1 201 Created
Cache-Control: no-cache
Pragma: no-cache
Content-Type: application/json; charset=utf-8
Expires: -1
Location: http://localhost:49535/api/customdata/0
Server: Microsoft-IIS/8.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?RDpcQ29kZVxzZW1wZl9tZXJjdXJ5cmVzdGFwaVxNZXJjdXJ5UmVzdEFwaVxhcGlcbG95YWx0eVxjdXN0b21kYXRh?=
X-Powered-By: ASP.NET
Date: Fri, 01 Mar 2013 23:29:45 GMT
Content-Length: 170
   {
      "Action":null,
      "BonusForEmail":0,
      "BonusForReg":0,
      "BonusForSocialMedia":0,
      "BonusForSurvey":0,
      "BusinessEmail":null,
      "BusinessName":null,
      "BusinessUrl":null,
      "Goal":0,
      "Incentive":null,
      "KeywordId":0,
      "KeywordName":null,
      "KioskAreaCode":false,
      "LogoUrl":null,
      "Options":null,
      "RegAreaCode":null,
      "RegImg1024x768":null,
      "RegImg1280x1024":null,
      "RegImg800x600":null,
      "ShortCode":null,
      "Units":null
   }

GET
GET http://localhost:49535/api/loyalty/customdata?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a HTTP/1.1
User-Agent: Fiddler
Host: localhost:49535


HTTP/1.1 200 OK
Cache-Control: no-cache
Pragma: no-cache
Content-Type: application/json; charset=utf-8
Expires: -1
Server: Microsoft-IIS/8.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?RDpcQ29kZVxzZW1wZl9tZXJjdXJ5cmVzdGFwaVxNZXJjdXJ5UmVzdEFwaVxhcGlcbG95YWx0eVxjdXN0b21kYXRh?=
X-Powered-By: ASP.NET
Date: Fri, 01 Mar 2013 23:28:18 GMT
Content-Length: 343
[
   {
      "CreateDate":"0001-01-01T00:00:00",
      "CustomerId":0,
      "DataGroup":null,
      "DataName":null,
      "DataType":0,
      "DataValue":null,
      "Id":0,
      "KeywordId":0,
      "LastUpdate":"0001-01-01T00:00:00"
   },
   {
      "CreateDate":"0001-01-01T00:00:00",
      "CustomerId":0,
      "DataGroup":null,
      "DataName":null,
      "DataType":0,
      "DataValue":null,
      "Id":0,
      "KeywordId":0,
      "LastUpdate":"0001-01-01T00:00:00"
   }
]
PUT
POST http://localhost:49535/api/loyalty/customdata?id=1001&customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a HTTP/1.1
User-Agent: Fiddler
Host: localhost:49535
Content-Type: application/json; charset=utf-8
Content-Length: 556

   {
      "Action":null,
      "BonusForEmail":0,
      "BonusForReg":0,
      "BonusForSocialMedia":0,
      "BonusForSurvey":0,
      "BusinessEmail":null,
      "BusinessName":null,
      "BusinessUrl":null,
      "Goal":0,
      "Incentive":null,
      "KeywordId":0,
      "KeywordName":null,
      "KioskAreaCode":false,
      "LogoUrl":null,
      "Options":null,
      "RegAreaCode":null,
      "RegImg1024x768":null,
      "RegImg1280x1024":null,
      "RegImg800x600":null,
      "ShortCode":null,
      "Units":null
   }

HTTP/1.1 201 Created
Cache-Control: no-cache
Pragma: no-cache
Content-Type: application/json; charset=utf-8
Expires: -1
Location: http://localhost:49535/api/customdata/0
Server: Microsoft-IIS/8.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?RDpcQ29kZVxzZW1wZl9tZXJjdXJ5cmVzdGFwaVxNZXJjdXJ5UmVzdEFwaVxhcGlcbG95YWx0eVxjdXN0b21kYXRh?=
X-Powered-By: ASP.NET
Date: Fri, 01 Mar 2013 23:30:29 GMT
Content-Length: 170
   {
      "Action":null,
      "BonusForEmail":0,
      "BonusForReg":0,
      "BonusForSocialMedia":0,
      "BonusForSurvey":0,
      "BusinessEmail":null,
      "BusinessName":null,
      "BusinessUrl":null,
      "Goal":0,
      "Incentive":null,
      "KeywordId":0,
      "KeywordName":null,
      "KioskAreaCode":false,
      "LogoUrl":null,
      "Options":null,
      "RegAreaCode":null,
      "RegImg1024x768":null,
      "RegImg1280x1024":null,
      "RegImg800x600":null,
      "ShortCode":null,
      "Units":null
   }

DELETE
DELETE http://localhost:49535/api/loyalty/customdata?id=1001&customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a HTTP/1.1
User-Agent: Fiddler
Host: localhost:49535
Content-Type: application/json; charset=utf-8
Content-Length: 0


HTTP/1.1 204 No Content
Cache-Control: no-cache
Pragma: no-cache
Expires: -1
Server: Microsoft-IIS/8.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?RDpcQ29kZVxzZW1wZl9tZXJjdXJ5cmVzdGFwaVxNZXJjdXJ5UmVzdEFwaVxhcGlcbG95YWx0eVxjdXN0b21kYXRh?=
X-Powered-By: ASP.NET
Date: Fri, 01 Mar 2013 23:31:02 GMT


