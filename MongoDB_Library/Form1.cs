using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace MongoDB_Library
{
    public partial class Form1 : Form
    {
        private const string connectionString = "mongodb://localhost:27017";
        public Book book { get; set; }
        public Form1()
        {
            InitializeComponent();
            MongoDBConnect();
        }
        //连接
        private void MongoDBConnect()
        {
            // 连接到一个MongoServer上  
            MongoServer server = MongoServer.Create(connectionString);
            // 打开数据库testdb  
            MongoDatabase db = server.GetDatabase("testdb");

            // 获取集合employees  
            MongoCollection<Book> collection = db.GetCollection<Book>("Library");
            //将数据绑定到datagridview中           
            dataGridView1.DataSource = new BindingList<Book>(db.GetCollection<Book>("Library").FindAll().ToList());
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {      
            DialogResult dr;
            dr = MessageBox.Show("Are you sure?", "Library", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {

            }
            else
            {
                e.Cancel = true;
            }        
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            tboxBookName.Clear();
            tboxBorrowDate.Clear();
            tboxName.Clear();
            tboxRetrunDate.Clear();
        }

        private bool insert()
        {
            string bookname = tboxBookName.Text; 
            string name = tboxName.Text;
            string borrowdate = tboxBorrowDate.Text;
            string returndate = tboxRetrunDate.Text;           
      
            if ((bookname == "") || (name == "") || (borrowdate == ""))
            {          
                return false;
            }
            else
            {
                MongoServer server = MongoServer.Create(connectionString);
                MongoDatabase db = server.GetDatabase("testdb");
                MongoCollection<Book> collection = db.GetCollection<Book>("Library");

                Book book = new Book();
                book.Name = name;
                book.BorrowedBookName = bookname;
                book.BorrowDate = borrowdate;
                book.ReturnDate = returndate;
                collection.Insert(book);                
                dataGridView1.DataSource = new BindingList<Book>(db.GetCollection<Book>("Library").FindAll().ToList());
                return true;
            }  
                
        }

        private void btinsert_Click(object sender, EventArgs e)
        {
            if (this.insert())
            {
                MessageBox.Show("添加成功!");
                tboxBookName.Clear();
                tboxBorrowDate.Clear();
                tboxName.Clear();
                tboxRetrunDate.Clear();

            }
            else
            {
                MessageBox.Show("请输入完整的记录！");
            }
        }
        //Remove a item.
        private bool drop()
        {
             
            MongoServer server = MongoServer.Create(connectionString);       
            MongoDatabase db = server.GetDatabase("testdb");          
            MongoCollection<Book> collection = db.GetCollection<Book>("Library");
            if (tboxName.Text == "")
            {
                DialogResult dr = MessageBox.Show("确定要删除所有记录吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    collection.Drop();
                    dataGridView1.DataSource = new BindingList<Book>(db.GetCollection<Book>("Library").FindAll().ToList());
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                DialogResult dr = MessageBox.Show("确定要删除吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    string bookname = tboxBookName.Text;
                    string name = tboxName.Text;
                    string borrowdate = tboxBorrowDate.Text;
                    string returndate = tboxRetrunDate.Text;  

                    //删除，首先进行实体类的查询，获取id才能够删除。
                    var Book = new Book(Name = name);
                    collection.Insert(Book);
                    var id = Book.Id;
                    var query = Query<Book>.EQ(q => q.Name, name);
                    var queryid = Query<Book>.EQ(q => q.Id, id);
                    collection.Remove(query);
                    collection.Remove(queryid);
                  
                    dataGridView1.DataSource = new BindingList<Book>(db.GetCollection<Book>("Library").FindAll().ToList());
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        { 
            string name;
            string bookname;
            string borrowdate;
            string retrundate;
            if ((dataGridView1.CurrentRow.Cells[1].Value == null)
                || (dataGridView1.CurrentRow.Cells[2].Value == null)
                || (dataGridView1.CurrentRow.Cells[3].Value == null)
                || (dataGridView1.CurrentRow.Cells[4].Value == null))
            {
            }
            else 
            {
                name = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                bookname = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                borrowdate = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                retrundate = dataGridView1.CurrentRow.Cells[4].Value.ToString();

                tboxName.Text = name;
                tboxBookName.Text = bookname;
                tboxBorrowDate.Text = borrowdate;
                tboxRetrunDate.Text = retrundate;
            }
           
        }

        private void btnremove_Click(object sender, EventArgs e)
        {
            if (this.drop())
            {
                MessageBox.Show("删除成功!");
            }
            else
            {
                MessageBox.Show("删除失败！");
            }
        }

        private bool update()
        {
            string bookname = tboxBookName.Text;
            string name = tboxName.Text;
            string borrowdate = tboxBorrowDate.Text;
            string returndate = tboxRetrunDate.Text;

            if ((bookname == "") || (name == "") || (borrowdate == ""))
            {
                return false;
            }
            else
            {
                MongoServer server = MongoServer.Create(connectionString);
                MongoDatabase db = server.GetDatabase("testdb");
                MongoCollection<Book> collection = db.GetCollection<Book>("Library");

                Book book = new Book();
                book.Name = name;
                book.BorrowedBookName = bookname;
                book.BorrowDate = borrowdate;
                book.ReturnDate = returndate;

                var Book = new Book(Name = name);
                //collection.Insert(Book);
                var id = Book.Id;
                var query = Query<Book>.EQ(q => q.Name, name);
                var queryid = Query<Book>.EQ(q => q.Id, id);
              
                var update = Update<Book>.Set(q => q.Name, "Harry");
                
                collection.Remove(query);
                collection.Remove(queryid);
                collection.Insert(book);
                dataGridView1.DataSource = new BindingList<Book>(db.GetCollection<Book>("Library").FindAll().ToList());
             
                return true;
            }  
        }
        private void btnupdate_Click(object sender, EventArgs e)
        {
            if (this.update())
            {
                MessageBox.Show("更新成功!");
            }
            else
            {
                MessageBox.Show("更新失败！");
            }
        }

        private bool find()
        {
            string bookname = tboxBookName.Text;
            string name = tboxName.Text;
            string borrowdate = tboxBorrowDate.Text;
            string returndate = tboxRetrunDate.Text;
            if (name == "")
            {
                return false;
            }
            else
            {
                MongoServer server = MongoServer.Create(connectionString);
                MongoDatabase db = server.GetDatabase("testdb");
                MongoCollection<Book> collection = db.GetCollection<Book>("Library");
                              
                var Book = new Book(Name = name);
                var id = Book.Id;             
                var query = Query<Book>.EQ(q => q.Name, name);
                Book = collection.FindOne(query);
                if (Book == null)
                {
                    return false;
                }
                else 
                {
                    tboxName.Text = Book.Name.ToString();
                    tboxBookName.Text = Book.BorrowedBookName.ToString();
                    tboxBorrowDate.Text = Book.BorrowDate.ToString();
                    tboxRetrunDate.Text = Book.ReturnDate.ToString();
                    return true;
                }               
            }        
        }

        private void btnfind_Click(object sender, EventArgs e)
        {
            if (this.find())
            {               
            }
            else
            {
                MessageBox.Show("没有此条记录！");
            }
        }

       
    }
}
