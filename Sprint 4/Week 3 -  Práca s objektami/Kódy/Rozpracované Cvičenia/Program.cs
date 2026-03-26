
class BankAccount
{
    private string _ownerName;
    private double _balance;

    public BankAccount(string name, double balance = 0)
    {
        _ownerName = name;
        _balance = balance;
    }

    public void Deposit(double amount)
    {
        _balance += amount;
    }

    public void Withdraw(double amount)
    {
        if (amount <= _balance)
        {
            _balance -= amount;
            return;
        }
        Console.WriteLine($"Vyber presahuje zostatok o: {amount - _balance}");
    }

    public string PrintBalance()
    {
        return $"Current balance: {_balance}";
    }
    
}

class Product
{ 
    public string _name;
    public double _price;
    public bool _inStock;

    public Product(string name, double price, bool inStock)
    {
        _name = name;
        _price = price;
        _inStock = inStock;
    }
}

class ShoppingCart
{
    private List<Product> _cart;

    public ShoppingCart()
    {
        _cart = new List<Product>();
    }

    public void AddProduct(Product product)
    {
        _cart.Add(product);
    }

    public void RemoveProduct(string name)
    {

        for (int index = 0; index < _cart.Count - 1; index++)
        {
            Product product = _cart[index];
            if (product._name.Equals(name))
            {
                _cart.RemoveAt(index);
            }
        }
    }

    public double GetTotal()
    {
        double total = 0;
        foreach (var product in _cart)
        {
            total += product._price;
        }

        return total;
    }
    
    public void PrintCart()
    {
        foreach (var product in _cart)
        {
            Console.WriteLine(
                $"Product name: {product._name}, Price: {product._price}, inStock: {product._inStock}"
                );
        }
    }
}

class Student
{
    
    private string _name;
    private int _age;
    private int _grade;

    public Student(string meno, int vek, int znamka)
    {
        _name = meno;
        _age = vek;
        _grade = znamka;
    }

    public string Greet()
    {
        return $"Hi, {_name}";
    }

    public bool isPassing()
    {
        if (_grade > 4)
        {
            return false;
        }
        return true;
    }

    public string Describe()
    {
        return $"Student: {_name}, Passed: {isPassing()}, with grade: {_grade}";
    }
    
    
}

class Classroom
{
    private List<Student> _students;

    public Classroom()
    {
        _students = new List<Student>();
    }

    public void AddStudent(Student student)
    {
        _students.Add(student);
    }

    public List<Student> GetPassingStudents()
    {
        List<Student> passingStudents = new List<Student>();
        foreach (var student in _students)
        {
            if (student.isPassing())
            {
                passingStudents.Add(student);
            }
        }
        
        return passingStudents;
    }

    
    public List<Student> GetFailingStudents()
    {
        List<Student> failingStudents = new List<Student>();
        foreach (var student in _students)
        {
            if (!student.isPassing())
            {
                failingStudents.Add(student);
            }
        }
        
        return failingStudents;
    }

    public double GetAverageAge()
    {
        int total = 0;
        
        foreach (var student in _students)
        {
            total += student.
        }
    }

}


class Program
{
    static void Main()
    {
        /*Student s1 = new Student("Samuel", 22, 2);
        string odpoved = s1.Describe();
        Console.WriteLine(s1.Greet());
        Console.Write(odpoved);*/
        
        /*BankAccount lukas = new BankAccount("Lukas");
        Console.WriteLine(lukas.PrintBalance());
        lukas.Deposit(1000);
        Console.WriteLine(lukas.PrintBalance());
        lukas.Withdraw(500);
        Console.WriteLine(lukas.PrintBalance());
        lukas.Withdraw(600);
        Console.WriteLine(lukas.PrintBalance());*/
        ShoppingCart tesco_cart = new ShoppingCart();
        
        Product jablko = new Product("Apple",3, true);
        Product melon = new Product("watermelon",10, false);
        
        
        tesco_cart.AddProduct(jablko);
        tesco_cart.AddProduct(melon);
        tesco_cart.PrintCart();
        Console.WriteLine($"Hodnota kosika: {tesco_cart.GetTotal()}");
        tesco_cart.RemoveProduct("Apple");
        Console.WriteLine("Po vymazani jablka");
        tesco_cart.PrintCart();
        Console.WriteLine($"Hodnota kosika: {tesco_cart.GetTotal()}");
    }
}
