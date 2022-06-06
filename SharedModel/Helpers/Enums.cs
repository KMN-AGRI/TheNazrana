

public enum Status
{
	Pending,
	Active,
	Completed,
	Cancelled,
	Disabled
}

public enum AlertType
{
	Info,
	Warning,
	Error,
	Success
}

public enum TargetType
{
	Product,
	Order,
	User
}

public enum Events
{
	Ordered,
	Shipped,
	In_Transit,
	Delivered,
	Cancelled,
	Completed
}

public enum ResultOrder
{
	Latest,
	PriceHighToLow,
	PriceLowToHigh,
	DiscountHighToLow,
	DiscountLowToHigh

}