# VdwwdBrickLink
VdwwdBrickLink is a light-weight C# library that can make API calls to the Lego BrickLink API to retrieve order(s). Then the order status and payent status can be updated.


# Initialize the VdwwdBrickLink library
Set your API keys and tokens in VdwwdBrickLink library. This only needs to be done only once, like in the Application_Start() or MainWindow() of your application.

    protected void Application_Start()
    {
        //set the required variables
        VdwwdBrickLink.Variables.setVariables("ConsumerKey", "ConsumerSecret", "TokenValue", "TokenSecret");
  
        //you can optionally specify the api url
        VdwwdBrickLink.Variables.setVariables("ConsumerKey", "ConsumerSecret", "TokenValue", "TokenSecret", "ApiUrl");
    }


# Retrieve orders placed in your store
Get a list of orders placed in your store.
By default you will get all the orders but you can filter by status Completed, Pending or Purged.

    //get a list of all the orders
    List<VdwwdBrickLink.Classes.Order> orders = await VdwwdBrickLink.Orders.getOrders();
 
    //get a list of the orders filtered by a specific status
    List<VdwwdBrickLink.Classes.Order> orders = await VdwwdBrickLink.Orders.getOrders(VdwwdBrickLink.Enums.OrderFilter.Completed);


# Get a specific order
Get a single order placed in your store with the order ID.

    //get a single order with the order id
    VdwwdBrickLink.Classes.Order order = await VdwwdBrickLink.Orders.getOrder(12345678);


# Updating the status of an order
Update the status or payment status of an order with the order ID.
A paymemt status is simplified to either true or false, but BrickLink has more options if needed.
The status can be one of the following options: Pending, Completed, Packed, Paid, Processing, Ready, Shipped or Updated.

    //update the payment status of an order to either true or false
    bool result = await VdwwdBrickLink.Orders.updateOrderPayment(12345678, true);
 
    //update the status of an order
    bool result = await VdwwdBrickLink.Orders.updateOrderStatus(12345678, VdwwdBrickLink.Enums.OrderStatus.Packed);


# Receive the order updates posted to your Callback URL
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
