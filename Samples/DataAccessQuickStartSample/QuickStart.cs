using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlServerCe ;
using System.Data.Common ;
using OpenNETCF.ApplicationBlocks.Data;

namespace CFQuickStartSamples_CS
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmQuickStart : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnDatabase;
		private System.Windows.Forms.TextBox txtResult;
		private System.Windows.Forms.Button btnNonQuerySP;
		private System.Windows.Forms.Button btnNonQuery1;
		private System.Windows.Forms.Button btntransupdate;
		private System.Windows.Forms.Button btnSingle;
		private System.Windows.Forms.Button btnDataReader;
		private System.Windows.Forms.Button btnDataSet;
		internal System.Windows.Forms.Button btnSingleRow;
		private System.Windows.Forms.Panel panel1;
		internal System.Windows.Forms.Button btnFillDS;
		internal System.Windows.Forms.Button btnUpdateDS;
		private System.Windows.Forms.Button txtXML;
		private SqlCeConnection ssceconn ;

	
		public frmQuickStart()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			panel1.Enabled =false ;

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
		}
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnDatabase = new System.Windows.Forms.Button();
			this.txtResult = new System.Windows.Forms.TextBox();
			this.btnDataReader = new System.Windows.Forms.Button();
			this.btnNonQuerySP = new System.Windows.Forms.Button();
			this.btnNonQuery1 = new System.Windows.Forms.Button();
			this.btntransupdate = new System.Windows.Forms.Button();
			this.btnSingle = new System.Windows.Forms.Button();
			this.btnDataSet = new System.Windows.Forms.Button();
			this.btnSingleRow = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnFillDS = new System.Windows.Forms.Button();
			this.btnUpdateDS = new System.Windows.Forms.Button();
			this.txtXML = new System.Windows.Forms.Button();
			// 
			// btnDatabase
			// 
			this.btnDatabase.Location = new System.Drawing.Point(24, 16);
			this.btnDatabase.Size = new System.Drawing.Size(224, 24);
			this.btnDatabase.Text = "Create Database";
			this.btnDatabase.Click += new System.EventHandler(this.button1_Click);
			// 
			// txtResult
			// 
			this.txtResult.Location = new System.Drawing.Point(280, 8);
			this.txtResult.Multiline = true;
			this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtResult.Size = new System.Drawing.Size(336, 264);
			this.txtResult.Text = "";
			// 
			// btnDataReader
			// 
			this.btnDataReader.Location = new System.Drawing.Point(8, 264);
			this.btnDataReader.Size = new System.Drawing.Size(240, 24);
			this.btnDataReader.Text = "Retrieve Multiple Rows using DataReader";
			this.btnDataReader.Click += new System.EventHandler(this.btnSinleRow_Click);
			// 
			// btnNonQuerySP
			// 
			this.btnNonQuerySP.Location = new System.Drawing.Point(8, 296);
			this.btnNonQuerySP.Size = new System.Drawing.Size(248, 24);
			this.btnNonQuerySP.Text = "Execute Non Query With Stored Procedure";
			this.btnNonQuerySP.Click += new System.EventHandler(this.btnNonQuerySP_Click);
			// 
			// btnNonQuery1
			// 
			this.btnNonQuery1.Location = new System.Drawing.Point(8, 104);
			this.btnNonQuery1.Size = new System.Drawing.Size(240, 24);
			this.btnNonQuery1.Text = "Execute NonQuery";
			this.btnNonQuery1.Click += new System.EventHandler(this.btnNonQuery1_Click);
			// 
			// btntransupdate
			// 
			this.btntransupdate.Location = new System.Drawing.Point(8, 136);
			this.btntransupdate.Size = new System.Drawing.Size(240, 24);
			this.btntransupdate.Text = "Perform Transactional Update";
			this.btntransupdate.Click += new System.EventHandler(this.button1_Click_1);
			// 
			// btnSingle
			// 
			this.btnSingle.Location = new System.Drawing.Point(8, 168);
			this.btnSingle.Size = new System.Drawing.Size(240, 24);
			this.btnSingle.Text = "Look Up Single Item";
			this.btnSingle.Click += new System.EventHandler(this.btnSingle_Click);
			// 
			// btnDataSet
			// 
			this.btnDataSet.Location = new System.Drawing.Point(8, 232);
			this.btnDataSet.Size = new System.Drawing.Size(240, 24);
			this.btnDataSet.Text = "Retrieve Multiple Rows using DataSet";
			this.btnDataSet.Click += new System.EventHandler(this.btnDataSet_Click);
			// 
			// btnSingleRow
			// 
			this.btnSingleRow.Location = new System.Drawing.Point(8, 200);
			this.btnSingleRow.Size = new System.Drawing.Size(240, 24);
			this.btnSingleRow.Text = "Retrieve Single Row";
			this.btnSingleRow.Click += new System.EventHandler(this.cmdSample3_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnFillDS);
			this.panel1.Controls.Add(this.btnUpdateDS);
			this.panel1.Controls.Add(this.txtXML);
			this.panel1.Controls.Add(this.btnNonQuery1);
			this.panel1.Controls.Add(this.btntransupdate);
			this.panel1.Controls.Add(this.btnSingle);
			this.panel1.Controls.Add(this.btnSingleRow);
			this.panel1.Controls.Add(this.btnDataSet);
			this.panel1.Controls.Add(this.btnDataReader);
			this.panel1.Controls.Add(this.btnNonQuerySP);
			this.panel1.Location = new System.Drawing.Point(13, 56);
			this.panel1.Size = new System.Drawing.Size(259, 328);
			// 
			// btnFillDS
			// 
			this.btnFillDS.Location = new System.Drawing.Point(8, 8);
			this.btnFillDS.Size = new System.Drawing.Size(240, 24);
			this.btnFillDS.Text = "Fill DataSet";
			this.btnFillDS.Click += new System.EventHandler(this.btnFillDS_Click_1);
			// 
			// btnUpdateDS
			// 
			this.btnUpdateDS.Location = new System.Drawing.Point(8, 40);
			this.btnUpdateDS.Size = new System.Drawing.Size(240, 24);
			this.btnUpdateDS.Text = "Update data source";
			this.btnUpdateDS.Click += new System.EventHandler(this.btnUpdateDS_Click);
			// 
			// txtXML
			// 
			this.txtXML.Location = new System.Drawing.Point(8, 72);
			this.txtXML.Size = new System.Drawing.Size(240, 24);
			this.txtXML.Text = "Retrieve XML Data";
			this.txtXML.Click += new System.EventHandler(this.txtXML_Click_1);
			// 
			// frmQuickStart
			// 
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.txtResult);
			this.Controls.Add(this.btnDatabase);
			this.Text = "DAAB QuickStart";
			this.Load += new System.EventHandler(this.frmQuickStart_Load);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>

		static void Main() 
		{
			Application.Run(new frmQuickStart());
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			
			//If the file "CEDataAccess.sdf" exists, delete it.

			if (System.IO.File.Exists("\\My Documents\\CEDataAccess.sdf"))
					System.IO.File.Delete("\\My Documents\\CEDataAccess.sdf");

 			//Create the new database.

			SqlCeEngine engine = new SqlCeEngine("Data Source = \\My Documents\\CEDataAccess.sdf");
			engine.CreateDatabase();

			//Create a connection to the new database.
			ssceconn = new SqlCeConnection("Data Source = \\My Documents\\CEDataAccess.sdf");

			//Create the createTable command on the connection.
			SqlCeCommand createTable = ssceconn.CreateCommand();            
			createTable.CommandText = "Create TABLE Customer(cust_id int IDENTITY(1,1) PRIMARY KEY, f_name ntext, l_name ntext, address ntext, city ntext, state ntext, pin ntext, country ntext, phone ntext)";
			//Open the connection, execute the queries
			ssceconn.Open();
			createTable.ExecuteNonQuery();

			//Create the insertRow command on the connection.
			SqlCeCommand insertRow = ssceconn.CreateCommand();
			insertRow.CommandText = "INSERT INTO Customer(f_name, l_name, address, city, state, pin, country, phone) VALUES ('Vandana', 'Singal', '102, Sai Madhavi Enclave', 'Hyderabad', 'AP', '500016', 'India', '919866224567')";
			insertRow.ExecuteNonQuery();
			insertRow.CommandText = "INSERT INTO Customer(f_name, l_name, address, city, state, pin, country, phone) VALUES ('Rajeev', 'Agarwal', '103, Krishna Enclave', 'Hyderabad', 'AP', '500016', 'India', '919866334568')";
			insertRow.ExecuteNonQuery();
			insertRow.CommandText = "INSERT INTO Customer(f_name, l_name, address, city, state, pin, country, phone) VALUES ('Anurag', 'Rastogi', '101, Krishna Enclave', 'Hyderabad', 'AP', '500016', 'India', '9166789876')";
			insertRow.ExecuteNonQuery();
			MessageBox.Show("Database Created");
			btnDatabase.Enabled =false ;
			panel1.Enabled =true ;

		}

		private void frmQuickStart_Load(object sender, System.EventArgs e)
		{
		
		}

		private void txtXML_Click(object sender, System.EventArgs e)
		{
			
		}

		private void btnSinleRow_Click(object sender, System.EventArgs e)
		{
				SqlCeDataReader obj_Reader=SqlHelper.ExecuteReader(ssceconn,CommandType.Text ,"Select * from Customer" );
					string str_result="";
				while(obj_Reader.Read())
				{
					str_result=str_result+ "Customer ID : " + obj_Reader.GetInt32(0).ToString()+"\r\n";
					str_result=str_result+ "Name : " + obj_Reader.GetString(1)+" " + obj_Reader.GetString(2)+"\r\n" ;
					str_result=str_result+ "Address : " + obj_Reader.GetString(3)+"\r\n";
					str_result=str_result+ "City: " + obj_Reader.GetString(4)+"\r\n";
					str_result=str_result+ "State : " + obj_Reader.GetString(5)+"\r\n";
					str_result=str_result+ "Zip : " + obj_Reader.GetString(6)+"\r\n";
					str_result=str_result+ "Country : " + obj_Reader.GetString(7)+"\r\n";
					str_result=str_result+ "Phone : " + obj_Reader.GetString(8)+"\r\n";
					str_result=str_result+ "\r\n";
                }
					txtResult.Text =str_result;
						
				
						//(ssceconn ,CommandType.Text ,"Select * fro"
		}

		private void btnNonQuerySP_Click(object sender, System.EventArgs e)
		{
			SqlCeParameter  paramFromAcc = new SqlCeParameter ("@AccountNo",SqlDbType.Int);
			paramFromAcc.Value = 11;
			try
			{
				SqlHelper.ExecuteNonQuery(ssceconn, CommandType.StoredProcedure, "Debit", paramFromAcc);
			}
			catch(Exception ex)
			{
				txtResult.Text ="Exception Occured: " + ex.Message; 
			}
		}

		private void btnNonQuery1_Click(object sender, System.EventArgs e)
		{
			int Records_Affected=0;
			Records_Affected =SqlHelper.ExecuteNonQuery(ssceconn, CommandType.Text,"Update Customer Set l_name='Montini'"); 
			txtResult.Text =Records_Affected.ToString ()+ " " + "Records Affected";
			
		}

		private void button1_Click_1(object sender, System.EventArgs e)
		{
			SqlCeTransaction trans = ssceconn.BeginTransaction();
			
				try
				{
					int Records_Affected=0;
					Records_Affected =SqlHelper.ExecuteNonQuery(ssceconn, CommandType.Text,"Update Customer Set l_name='Montini'"); 
					SqlHelper.ExecuteNonQuery(ssceconn, CommandType.Text,"Update Customer Set t_name='Montini'"); 
					trans.Commit(); 
					string str_return = "Transaction Completed.  \r\n";
					txtResult.Text =str_return+ Records_Affected.ToString ()+ " " + "Records Affected"+"\r\n";
				}
				catch (Exception ex)
				{
					// throw exception						
					trans.Rollback();
					txtResult.Text = "Error Occured. Transaction Rolled Back";
					string str_msg= ex.Message;						
				}
			
		}

		private void btnSingle_Click(object sender, System.EventArgs e)
		{
			string str_name;
			
			str_name = (string)SqlHelper.ExecuteScalar(ssceconn,CommandType.Text ,"Select f_name from Customer where cust_id=1");
			txtResult.Text = str_name ;				
			
		}

		private void btnDataSet_Click(object sender, System.EventArgs e)
		{
			DataSet ds=SqlHelper.ExecuteDataset(ssceconn,CommandType.Text,"select * from Customer" );
			txtResult.Text =ds.GetXml();
		}

		private void cmdSample3_Click(object sender, System.EventArgs e)
		{
			SqlCeDataReader obj_Reader=SqlHelper.ExecuteReader(ssceconn,CommandType.Text ,"Select * from Customer where cust_id=2" );
			string str_result="";
			while(obj_Reader.Read())
			{
				str_result=str_result+ "Customer ID : " + obj_Reader.GetInt32(0).ToString()+"\r\n";
				str_result=str_result+ "Name : " + obj_Reader.GetString(1)+" " + obj_Reader.GetString(2)+"\r\n" ;
				str_result=str_result+ "Address : " + obj_Reader.GetString(3)+"\r\n";
				str_result=str_result+ "City: " + obj_Reader.GetString(4)+"\r\n";
				str_result=str_result+ "State : " + obj_Reader.GetString(5)+"\r\n";
				str_result=str_result+ "Zip : " + obj_Reader.GetString(6)+"\r\n";
				str_result=str_result+ "Country : " + obj_Reader.GetString(7)+"\r\n";
				str_result=str_result+ "Phone : " + obj_Reader.GetString(8)+"\r\n";
				str_result=str_result+ "\r\n";
			}
			txtResult.Text =str_result;
		}

		private void btnUpdateDS_Click(object sender, System.EventArgs e)
		{
			// DataSet that will hold the returned results
			DataSet ds = new DataSet();
			
			// Call FillDataset static method of SqlHelper class that fills a Dataset
			SqlHelper.FillDataset(ssceconn,CommandType.Text ,"Select * from Customer where cust_id >1",ds,new string[]{"Customer"});
					
			// DataTable that hold the returned results
			DataTable table = ds.Tables["Customer"];

			// Add a new records to existing DataSet
			DataRow addedRow = table.Rows.Add(new object[] {4,"New FirstName","New LastName","New Address","New City","New State","New PIN","New Country","New Phone"});
            
			// Modify a existing product
			table.Rows[1]["f_name"] = "Modified FirstName";

			// Create the command that will be used for insert operations
			SqlCeCommand insertCommand = SqlHelper.CreateCommand(ssceconn,CommandType.Text ,"Insert into Customer values('NewFirstName1','New LastName1,'New Address1','New City1','New State1','New PIN1 ',' New Country1','New Phone1')");
				
                
			// Create the command that will be used for update operations
			// The stored procedure also performs a SELECT to allow updating the DataSet with other changes (Identity columns, changes performed by triggers, etc)
			
			SqlCeCommand  updateCommand = SqlHelper.CreateCommand( ssceconn,CommandType.Text ,"Update Customer set City='Redmond' where cust_id=3");
            
			// Create the command that will be used for delete operations
			
			SqlCeCommand deleteCommand = SqlHelper.CreateCommand( ssceconn,CommandType.Text ,"Delete Customer where cust_id=1" );

			try
			{
				// Update the data source with the DataSet changes
				SqlHelper.UpdateDataset(insertCommand, deleteCommand, updateCommand, ds, "Customer");

				//Get the new product id. This id was generated in the data source
				txtResult.Text = "Completed .";
			}
			catch(DBConcurrencyException)
			{
				MessageBox.Show("A concurrency error has ocurred while trying to update the data source", "Application Error");
				txtResult.Text = "The following rows wasn´t updated: ";
				foreach( DataRow currentRow in table.Rows )
				{
					if ( currentRow.RowState != DataRowState.Unchanged )
					{
						txtResult.Text += "\r\n" + "Cust ID: " + currentRow["cust_id"].ToString() + 
							" Name: " + currentRow["f_name"].ToString()+" "+ currentRow["l_name"].ToString();
					}
				}
			}
		}

		private void btnFillDS_Click_1(object sender, System.EventArgs e)
		{
			DataSet ds = new DataSet();
            			
			// Call FillDataset static method of SqlHelper class that fills a existing Dataset
			SqlHelper.FillDataset(ssceconn,CommandType.Text,"Select * from Customer where cust_id=1",ds,new string[]{"Customer"});
				
			// Get XML representation of the dataset and display results in text box
			txtResult.Text = ds.GetXml();
		}

		private void txtXML_Click_1(object sender, System.EventArgs e)
		{
			SqlCeDataAdapter adp=new SqlCeDataAdapter();
			string str_result=SqlHelper.ExecuteXml(ssceconn,CommandType.Text,"Select * from Customer");
			txtResult.Text =str_result;		
		}

		
					
	}
}
