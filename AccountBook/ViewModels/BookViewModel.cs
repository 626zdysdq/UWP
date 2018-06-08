using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBook.ViewModels
{
    public class BookViewModel
    {
        public static SQLiteConnection sqlConn;          //数据库
        public static BookViewModel Instance
        {
            get
            {
                if (instance == null) instance = new BookViewModel();
                return instance;
            }
        }
        private static BookViewModel instance;           //ViewModel的单例
        private Models.Book selectedBook = default(Models.Book);                      //当前选中的账本
        public Models.Book SelectedBook { get { return selectedBook; } set { this.selectedBook = value; } }

        private ObservableCollection<Models.Book> allBooks = new ObservableCollection<Models.Book>();   //视图中显示的所有账本
        public ObservableCollection<Models.Book> AllBooks { get { return this.allBooks; } }


        public static List<Models.Account> AllAccounts
        {
            get
            {
                List<Models.Account> allAccounts = new List<Models.Account>();
                using (var statement = sqlConn.Prepare(@"SELECT * FROM Item"))
                {
                    while (SQLiteResult.ROW == statement.Step())
                    {
                        //一条记录的类型为空，则代表是一个新的账本的记录而不是一条账目的记录
                        if ((string)statement[5] != "")
                        {
                            Models.Account account = new Models.Account(DateTime.Parse((string)statement[3]), (Int64)statement[4] == 1, (string)statement[5], (double)statement[6],
                                        (string)statement[7], (string)statement[1]);    //使用账本名称作为id，以便在磁贴中标明
                            allAccounts.Add(account);
                        }
                    }
                }
                allAccounts.Sort(delegate (Models.Account x, Models.Account y)
                {
                    return x.time.Date.CompareTo(y.time.Date);
                });
                return allAccounts;
            }
        }


        public BookViewModel()
        {
            //创建数据库
            CreateDatabase();
            //获取数据库的信息
            GetItems();
        }


        //添加一个账本
        public void AddBook(string name, double budget)
        {
            Models.Book toAdd = new Models.Book(name, budget);
            allBooks.Add(toAdd);
            //数据库插入一个账本
            InsertItem(toAdd);
        }

        //更新账本
        public void UpdateBook(string name, double budget)
        {
            if (selectedBook != null)
            {
                selectedBook.name = name;
                selectedBook.monthlyBudget = budget;
            }
            //数据库更新选中账本
            UpdateItem(selectedBook);
            //selectedBook = null;
        }
        //删除账本
        public void DeleteBook()
        {
            //数据库删除选中账本
            DeleteItem(selectedBook);
            allBooks.Remove(selectedBook);
            selectedBook = null;        
        }
        //sql创建一个数据库
        public void CreateDatabase()
        {
            sqlConn = new SQLiteConnection("AccountBook.db");
            string sql = @"CREATE TABLE IF NOT EXISTS
                                        Item (Id TEXT,
                                                Name    TEXT,
                                                AccountId   TEXT PRIMARY KEY,
                                                Date    TEXT,
                                                receiptOrPay INTEGER,
                                                Type   TEXT,
                                                Money    REAL,
                                                Remark  TEXT
                                                );";
            using (var statement = sqlConn.Prepare(sql))
            {
                statement.Step();
            }
        }
        //得到sql中的数据
        public void GetItems()
        {
            using (var statement = sqlConn.Prepare(@"SELECT * FROM Item"))
            {
                while (SQLiteResult.ROW == statement.Step())
                {
                    //一条记录的类型为空，则代表是一个新的账本的记录而不是一条账目的记录
                    if ((string)statement[5] == "")
                    {
                        Models.Book tempBook = new Models.Book((string)statement[1], (double)statement[6], (string)statement[0]);
                        allBooks.Add(tempBook);
                    }
                    else
                    {
                        foreach (Models.Book book in allBooks)
                        {
                            //寻找相同账本id的账本，并加入该账本
                            if(book.id == (string)statement[0])
                            {
                                book.AddAccount(DateTime.Parse((string)statement[3]), (Int64)statement[4] == 1, (string)statement[5], (double)statement[6],
                                    (string)statement[7], (string)statement[2]);
                            }
                        }
                    }
                }
            }
        }
        //sql插入一条账本记录
        public void InsertItem(Models.Book item)
        {
            if (item == null) return;
            using (var statement = sqlConn.Prepare(@"INSERT INTO Item (Id, Name, AccountId, Date, receiptOrPay, Type,Money,Remark) VALUES (?, ?, ?, ?, ?, ?, ?, ?)"))
            {
                statement.Bind(1, item.id);
                statement.Bind(2, item.name);
                statement.Bind(3, Guid.NewGuid().ToString());
                statement.Bind(4, "");
                statement.Bind(5, 0);
                statement.Bind(6, "");
                statement.Bind(7, item.monthlyBudget);
                statement.Bind(8, "");
                statement.Step();
            }
        }
        //sql插入一条账目记录
        public void InsertItem(Models.Account item)
        {
            if (item == null) return;
            using (var statement = sqlConn.Prepare(@"INSERT INTO Item (Id, Name, AccountId, Date, receiptOrPay, Type,Money,Remark) VALUES (?, ?, ?, ?, ?, ?, ?, ?)"))
            {
                statement.Bind(1, SelectedBook.id);
                statement.Bind(2, SelectedBook.name);
                statement.Bind(3, item.id);
                statement.Bind(4, item.time.Date.ToString());
                statement.Bind(5, item.receiptOrPay ? 1 : 0);
                statement.Bind(6, item.type);
                statement.Bind(7, item.money);
                statement.Bind(8, item.remark);
                statement.Step();
            }
        }
        //sql删除一个账本
        public void DeleteItem(Models.Book item)
        {
            if (item == null) return;
            using (var statement = sqlConn.Prepare(@"DELETE FROM Item WHERE Id = ?"))
            {
                statement.Bind(1, item.id);
                statement.Step();
            }
        }
        //sql删除一条账目
        public void DeleteItem(Models.Account item)
        {
            if (item == null) return;
            using (var statement = sqlConn.Prepare(@"DELETE FROM Item WHERE AccountId = ?"))
            {
                statement.Bind(1, item.id);
                statement.Step();
            }
        }
        //sql更新一个账本
        public void UpdateItem(Models.Book item)
        {
            if (item == null) return;
            using (var statement = sqlConn.Prepare(@"UPDATE Item SET Name = ? WHERE Id = ?"))
            {
                statement.Bind(1, item.name);
                statement.Bind(2, item.id);
                statement.Step();
            }
        }
        //sql更新一条账目
        public void UpdateItem(Models.Account item)
        {
            if (item == null) return;
            using (var statement = sqlConn.Prepare(@"UPDATE Item SET Date = ?,  receiptOrPay = ?, Type = ?, Money = ?, Remark = ? WHERE AccountId = ?"))
            {
                statement.Bind(1, item.time.Date.ToString());
                statement.Bind(2, item.receiptOrPay ? 1 : 0);
                statement.Bind(3, item.type);
                statement.Bind(4, item.money);
                statement.Bind(5, item.remark);
                statement.Bind(6, item.id);
                statement.Step();
            }
        }

        //搜索
        public StringBuilder Search(String word)
        {
            StringBuilder result = new StringBuilder();
            word = "%" + word + "%";
            using (var statement = sqlConn.Prepare(@"SELECT Name, Type, Date, Money, Remark, receiptOrPay  FROM Item WHERE Name LIKE ? OR Type LIKE ? OR Date LIKE ? OR Money LIKE ? OR Remark LIKE ?"))
            {
                statement.Bind(1, word);
                statement.Bind(2, word);
                statement.Bind(3, word);
                statement.Bind(4, word);
                statement.Bind(5, word);
                while (SQLiteResult.ROW == statement.Step())
                {
                    string name = (String)statement[0];
                    string type = (String)statement[1];
                    
                    result.Append("账本：").Append(name);
                    if(type == "")
                    {
                        //这是一个账本记录
                        result.Append("\t\t每月预算：");
                        if ((double)statement[3] == -1) result.Append("无上限");
                        else result.Append((double)statement[3]);
                    }
                    else
                    {
                        //这是一个账目记录
                        string date = ((String)statement[2]).Split()[0];
                        string money = ((Int64)statement[5] == 0 ? "+" : "-") + (double)statement[3];
                        string remark = (String)statement[4];

                        result.Append("\t\t账目类型：").Append(type);
                        result.Append("\t");
                        if (type.Length < 3)
                            result.Append("\t");
                        result.Append("日期：").Append(date);
                        result.Append("\t");
                        if (date.Length < 10)
                            result.Append("\t");
                        result.Append("金额：").Append(money).Append("\t\t备注：").Append(remark);
                    }
                    result.Append(Environment.NewLine);
                }
            }
            return result;
        }
    }
}
