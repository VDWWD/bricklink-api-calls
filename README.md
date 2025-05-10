# VdwwdBrickLink
VdwwdBrickLink is a light-weight C# library that can make API calls to the Lego BrickLink API to retrieve order(s). Then the order status and payent status can be updated. Also get Catalog Item and color information from BrickLink.


## Generate your keys and tokens
1. Create a [BrickLink](https://www.bricklink.com/) account if you do not have one and then become a [Seller](https://www.bricklink.com/help.asp?helpID=2440).
2. Go to the [BrickLink API Consumer Registration](https://www.bricklink.com/v2/api/register_consumer.page) page.
3. Add a Callback URL to which BrickLink will post the order updates.
4. Generate your keys and tokens. Note: You will need a TokenValue and TokenSecret per IP address.
5. Read the [API guide](https://www.bricklink.com/v3/api.page).
6. If you want to update the order status and payment status separately then you have to enable the option 'order payment status from order status' on the [My Orders Settings](https://www.bricklink.com/orderSettings.asp) page.


## Initialize the VdwwdBrickLink library
Set your API keys and tokens in VdwwdBrickLink library. This only needs to be done only once, like in the Application_Start() or MainWindow() of your application.

    protected void Application_Start()
    {
        //set the required variables
        VdwwdBrickLink.Variables.setVariables("ConsumerKey", "ConsumerSecret", "TokenValue", "TokenSecret");
  
        //you can optionally specify the api url
        VdwwdBrickLink.Variables.setVariables("ConsumerKey", "ConsumerSecret", "TokenValue", "TokenSecret", "ApiUrl");
    }


## Retrieve orders placed in your store
Get a list of orders placed in your store.
By default you will get all the orders but you can filter by status Completed, Pending or Purged.

    //get a list of all the orders
    List<VdwwdBrickLink.Classes.Order> orders = await VdwwdBrickLink.Orders.getOrders();
 
    //get a list of the orders filtered by a specific status
    List<VdwwdBrickLink.Classes.Order> orders = await VdwwdBrickLink.Orders.getOrders(VdwwdBrickLink.Enums.OrderFilter.Completed);


## Get a specific order
Get a single order placed in your store with the order ID.

    //get a single order with the order id
    VdwwdBrickLink.Classes.Order order = await VdwwdBrickLink.Orders.getOrder(12345678);


## Updating the status of an order
Update the status or payment status of an order with the order ID.
A paymemt status is simplified to either true or false, but BrickLink has more options if needed.
The status can be one of the following options: Pending, Completed, Packed, Paid, Processing, Ready, Shipped or Updated.

    //update the payment status of an order to either true or false
    bool result = await VdwwdBrickLink.Orders.updateOrderPayment(12345678, true);
 
    //update the status of an order
    bool result = await VdwwdBrickLink.Orders.updateOrderStatus(12345678, VdwwdBrickLink.Enums.OrderStatus.Packed);


## Receive the order updates posted to your Callback URL
BrickLink can post order updates to your specified Callback URL. These posts are a List of Push Notifications.
Depending on the application type used you can receive them in multiple ways. Below are two examples. VdwwdBrickLink cannot receive them for you but it can convert the posted Stream to a list of notifications.

    [OperationContract]
    [WebInvoke(Method = "POST", UriTemplate = "YourLocalUrl")]
    public void MyBrickLinkMethod(Stream stream)
    {
        //convert the posted stream to a list of notifications
        List<VdwwdBrickLink.Classes.PushNotification> notifications = VdwwdBrickLink.Helpers.readPostedStream(stream);
 
        //handle the order updates here
    }
 
    [HttpPost]
    [Route("YourLocalUrl")]
    public void MyBrickLinkMethod(List<VdwwdBrickLink.Classes.PushNotification> notifications)
    {
        //handle the order updates here
    }


## Get Catalog Item information
API GET actions to receive information on a specific Lego part.

    //get a lego part with the part number
    VdwwdBrickLink.Classes.CatalogItem item = await VdwwdBrickLink.CatalogItems.GetItem(VdwwdBrickLink.Enums.CatalogItemType.Minifig, "dis178");
     
    //get all available color of a lego part
    List<VdwwdBrickLink.Classes.CatalogItemColor> itemcolors = await VdwwdBrickLink.CatalogItems.GetItemColors(VdwwdBrickLink.Enums.CatalogItemType.Part, "3001");
     
    //get the image url of a part
    string itemimage = await VdwwdBrickLink.CatalogItems.GetItemImageUrl(VdwwdBrickLink.Enums.CatalogItemType.Part, "3001");
     
    //get the image url of a part in a specific color
    string itemimage = await VdwwdBrickLink.CatalogItems.GetItemImageUrl(VdwwdBrickLink.Enums.CatalogItemType.Part, "3001", 11);
     
    //get the average price of the part from the last 6 months sales
    VdwwdBrickLink.Classes.CatalogPrice prices = await VdwwdBrickLink.CatalogItems.GetPriceGuide(VdwwdBrickLink.Enums.CatalogItemType.Part, VdwwdBrickLink.Enums.CatalogItemPriceType.New, "32532", 7);


## Get Color information
API GET actions to receive global BrickLink color information.

    //get all the available colors
    List<VdwwdBrickLink.Classes.Color> color = await VdwwdBrickLink.Colors.GetColors();
     
    //get a specific color
    VdwwdBrickLink.Classes.Color colors = await VdwwdBrickLink.Colors.GetColor(11);
