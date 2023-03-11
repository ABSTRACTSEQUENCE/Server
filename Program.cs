using System.Text.Json;
List<Person> personlist = new();
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.Run(async (context) =>
{
	async void AddPerson()
	{
		try 
		{
			personlist = JsonSerializer.Deserialize<List<Person>>(File.ReadAllText("Users.json"));
		}
		catch (Exception){}
		File.WriteAllText("Users.json", "");
		Person dude = await context.Request.ReadFromJsonAsync<Person>();
		foreach (Person i in personlist) { if (dude.id == i.id) dude.id++; }
		personlist.Add(dude);
		File.AppendAllText("Users.json", JsonSerializer.Serialize(personlist)+"\n");
		await context.Response.WriteAsJsonAsync(dude);
	}
	async void DelPerson()
	{
		Person dude = await context.Request.ReadFromJsonAsync<Person>();
		personlist = JsonSerializer.Deserialize<List<Person>>(File.ReadAllText("Users.json"));
		File.WriteAllText("Users.json", "");
		personlist.Remove(dude);
		File.AppendAllText("Users.json", JsonSerializer.Serialize(personlist) + "\n");
	}
	async void GetPersonList()
	{
		await context.Response.WriteAsJsonAsync(File.ReadAllText("/Users.json"));
	}
app.Logger.LogInformation($"Path: { context.Request.Path }");
	switch(context.Request.Path)
	{
		case "/api":
			switch(context.Request.Method)
			{
				case"POST":
					AddPerson();
					break;
				case "DEL":
					DelPerson();
					break;
				case "GET":
					GetPersonList();
					break;
			}
			break;
		default:
			context.Response.ContentType = "text/html";
			await context.Response.SendFileAsync("index.html");
			break;

	}
});
app.Run();

public class Person
{
	public int id { get; set; }
	public string name { get; set; }
	public string s_name { get; set; }

	public Person(string name, string s_name) { this.name = name; this.s_name = s_name; }
	public Person () { }
	public override string ToString()
	{
		return $"Name: {name} Second Name:{s_name}";
	}
	//Пока работаем только с name и id, потом добвавить новые переменные
}


