
using System;
using System.Data;
using System.Xml;
using System.Collections;
using System.Data.Common;
using System.Reflection;
using System.Globalization ;

using DataWeb.TurboDB;

namespace OpenNETCF.ApplicationBlocks.Data.TurboDBServer
{
	/// <summary>
	/// The TurboDBHelper class is intended to encapsulate high performance, scalable best practices for 
	/// common uses of TurboDBClient
	/// </summary>
	public sealed class TurboDBHelper
	{
		#region private utility methods & constructors

		// Since this class provides only static methods, make the default constructor private to prevent 
		// instances from being created with "new TurboDBHelper()"
		private TurboDBHelper() {}

		/// <summary>
		/// This method is used to attach array of TurboDBParameters to a TurboDBCommand.
		/// 
		/// This method will assign a value of DbNull to any parameter with a direction of
		/// InputOutput and a value of null.  
		/// 
		/// This behavior will prevent default values from being used, but
		/// this will be the less common case than an intended pure output parameter (derived as InputOutput)
		/// where the user provided no input value.
		/// </summary>
		/// <param name="command">The command to which the parameters will be added</param>
		/// <param name="commandParameters">An array of TurboDBParameters to be added to command</param>
		private static void AttachParameters(TurboDBCommand command, TurboDBParameter[] commandParameters)
		{
			
			if( command == null ) throw new ArgumentNullException( "command" );
			if( commandParameters != null )
			{
				foreach (TurboDBParameter p in commandParameters)
				{
					if( p != null )
					{
						// Check for derived output value with no value assigned
						if ( ( p.Direction == ParameterDirection.InputOutput || 
							p.Direction == ParameterDirection.Input ) && 
							(p.Value == null))
						{
							p.Value = DBNull.Value;
						}
						command.Parameters.Add(p);
					}
				}
			}
		}


		/// <summary>
		/// This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
		/// to the provided command
		/// </summary>
		/// <param name="command">The TurboDBCommand to be prepared</param>
		/// <param name="connection">A valid TurboDBConnection, on which to execute this command</param>
		/// <param name="transaction">A valid TurboDBTransaction, or 'null'</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <param name="commandParameters">An array of TurboDBParameters to be associated with the command or 'null' if no parameters are required</param>
		/// <param name="mustCloseConnection"><c>true</c> if the connection was opened by the method, otherwose is false.</param>
		private static void PrepareCommand(TurboDBCommand command, TurboDBConnection connection, TurboDBTransaction transaction, CommandType commandType, string commandText, TurboDBParameter[] commandParameters, out bool mustCloseConnection )
		{
			if( command == null ) throw new ArgumentNullException( "command" );
			
			if(commandType == CommandType.StoredProcedure ) throw new ArgumentException("Stored Procedures are not supported.");
			
			// If the provided connection is not open, we will open it
			if (connection.State != ConnectionState.Open)
			{
				mustCloseConnection = true;
				connection.Open();
			}
			else
			{
				mustCloseConnection = false;
			}

			// Associate the connection with the command
			command.Connection = connection;

			// Set the command text (SQL statement)
			command.CommandText = commandText;

			// If we were provided a transaction, assign it
			if (transaction != null)
			{
				if( transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
				command.Transaction = transaction;
			}

			// Set the command type
			command.CommandType = commandType;

			// Attach the command parameters if they are provided
			if (commandParameters != null)
			{
				AttachParameters(command, commandParameters);
			}
			return;
		}

		#endregion private utility methods & constructors

		#region ExecuteNonQuery

		/// <summary>
		/// Execute a TurboDBCommand (that returns no resultset and takes no parameters) against the database specified in 
		/// the connection string
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(connString, CommandType.Text, "Insert into TableTransaction (OrderAmount) Values(500)");
		/// </remarks>
		/// <param name="connectionString">A valid connection string for a TurboDBConnection</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <returns>An int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
		{
			// Pass through the call providing null for the set of TurboDBParameters
			return ExecuteNonQuery(connectionString, commandType, commandText, (TurboDBParameter[])null);
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns no resultset) against the database specified in the connection string 
		/// using the provided parameters
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(connString, CommandType.Text, "Update TableTransaction set OrderAmount = 500 where ProdId=?", new TurboDBParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">A valid connection string for a TurboDBConnection</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <param name="commandParameters">An array of TurboDBParamters used to execute the command</param>
		/// <returns>An int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params TurboDBParameter[] commandParameters)
		{
			if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );

			// Create & open a TurboDBConnection, and dispose of it after we are done
			using (TurboDBConnection connection = new TurboDBConnection(connectionString))
			{

				// Call the overload that takes a connection in place of the connection string
				return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
			}
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns no resultset and takes no parameters) against the provided TurboDBConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(conn, CommandType.Text, "Insert into TableTransaction (OrderAmount) Values(500)");
		/// </remarks>
		/// <param name="connection">A valid TurboDBConnection</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <returns>An int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(TurboDBConnection connection, CommandType commandType, string commandText)
		{
			// Pass through the call providing null for the set of TurboDBParameters
			return ExecuteNonQuery(connection, commandType, commandText, (TurboDBParameter[])null);
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns no resultset) against the specified TurboDBConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(conn, CommandType.Text, "Update TableTransaction set OrderAmount = 500 where ProdId=?", new TurboDBParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connection">A valid TurboDBConnection</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <param name="commandParameters">An array of TurboDBParamters used to execute the command</param>
		/// <returns>An int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(TurboDBConnection connection, CommandType commandType, string commandText, params TurboDBParameter[] commandParameters)
		{	
			if( connection == null ) throw new ArgumentNullException( "connection" );

			// Create a command and prepare it for execution
			TurboDBCommand cmd = new TurboDBCommand();
			bool mustCloseConnection = false;
			PrepareCommand(cmd, connection, (TurboDBTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection );
    		
			// Finally, execute the command
			int retval = cmd.ExecuteNonQuery();
    		
			// Detach the TurboDBParameters from the command object, so they can be used again
			cmd.Parameters.Clear();
			if( mustCloseConnection )
				connection.Close();
			return retval;
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns no resultset and takes no parameters) against the provided TurboDBTransaction. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(trans, CommandType.Text, "Insert into TableTransaction (OrderAmount) Values(500)");
		/// </remarks>
		/// <param name="transaction">A valid TurboDBTransaction</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <returns>An int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(TurboDBTransaction transaction, CommandType commandType, string commandText)
		{
			// Pass through the call providing null for the set of TurboDBParameters
			return ExecuteNonQuery(transaction, commandType, commandText, (TurboDBParameter[])null);
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns no resultset) against the specified TurboDBTransaction
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(trans, CommandType.Text, "Select * from TableTransaction where ProdId=?", new TurboDBParameter("@prodid", 24));
		/// </remarks>
		/// <param name="transaction">A valid TurboDBTransaction</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <param name="commandParameters">An array of TurboDBParamters used to execute the command</param>
		/// <returns>An int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(TurboDBTransaction transaction, CommandType commandType, string commandText, params TurboDBParameter[] commandParameters)
		{
			if( transaction == null ) throw new ArgumentNullException( "transaction" );
			if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );

			// Create a command and prepare it for execution
			TurboDBCommand cmd = new TurboDBCommand();
			bool mustCloseConnection = false;
			PrepareCommand(cmd, (TurboDBConnection)transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );
    			
			// Finally, execute the command
			int retval = cmd.ExecuteNonQuery();
    			
			// Detach the TurboDBParameters from the command object, so they can be used again
			cmd.Parameters.Clear();
			return retval;
		}

		#endregion ExecuteNonQuery

		#region ExecuteDataset

		/// <summary>
		/// Execute a TurboDBCommand (that returns a resultset and takes no parameters) against the database specified in 
		/// the connection string. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(connString, CommandType.Text, "Select * from TableTransaction");
		/// </remarks>
		/// <param name="connectionString">A valid connection string for a TurboDBConnection</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <returns>A dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
		{
			// Pass through the call providing null for the set of TurboDBParameters
			return ExecuteDataset(connectionString, commandType, commandText, (TurboDBParameter[])null);
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns a resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(connString, CommandType.Text, "Select * from TableTransaction where ProdId=?", new TurboDBParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">A valid connection string for a TurboDBConnection</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <param name="commandParameters">An array of TurboDBParamters used to execute the command</param>
		/// <returns>A dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params TurboDBParameter[] commandParameters)
		{
			if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );

			// Create & open a TurboDBConnection, and dispose of it after we are done
			using (TurboDBConnection connection = new TurboDBConnection(connectionString))
			{
				// Call the overload that takes a connection in place of the connection string
				return ExecuteDataset(connection, commandType, commandText, commandParameters);
			}
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns a resultset and takes no parameters) against the provided TurboDBConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(conn, CommandType.Text, "Select * from TableTransaction");
		/// </remarks>
		/// <param name="connection">A valid TurboDBConnection</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <returns>A dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(TurboDBConnection connection, CommandType commandType, string commandText)
		{
			// Pass through the call providing null for the set of TurboDBParameters
			return ExecuteDataset(connection, commandType, commandText, (TurboDBParameter[])null);
		}
		
		/// <summary>
		/// Execute a TurboDBCommand (that returns a resultset) against the specified TurboDBConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(conn, CommandType.Text, "Select * from TableTransaction where ProdId=?", new TurboDBParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connection">A valid TurboDBConnection</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <param name="commandParameters">An array of TurboDBParamters used to execute the command</param>
		/// <returns>A dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(TurboDBConnection connection, CommandType commandType, string commandText, params TurboDBParameter[] commandParameters)
		{
			if( connection == null ) throw new ArgumentNullException( "connection" );

			// Create a command and prepare it for execution
			TurboDBCommand cmd = new TurboDBCommand();
			bool mustCloseConnection = false;
			PrepareCommand(cmd, connection, (TurboDBTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection );
    			
			// Create the DataAdapter & DataSet
			TurboDBDataAdapter da = new TurboDBDataAdapter(cmd);
				
			DataSet ds = new DataSet();
			ds.Locale  =CultureInfo.InvariantCulture;

			// Fill the DataSet using default values for DataTable names, etc
			da.Fill(ds);
			
			
			// Detach the TurboDBParameters from the command object, so they can be used again
			cmd.Parameters.Clear();

			if( mustCloseConnection )
				connection.Close();

			// Return the dataset
			return ds;

		}
		
		/// <summary>
		/// Execute a TurboDBCommand (that returns a resultset and takes no parameters) against the provided TurboDBTransaction. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(trans, CommandType.Text, "Select * from TableTransaction");
		/// </remarks>
		/// <param name="transaction">A valid TurboDBTransaction</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <returns>A dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(TurboDBTransaction transaction, CommandType commandType, string commandText)
		{
			// Pass through the call providing null for the set of TurboDBParameters
			return ExecuteDataset(transaction, commandType, commandText, (TurboDBParameter[])null);
		}
		
		/// <summary>
		/// Execute a TurboDBCommand (that returns a resultset) against the specified TurboDBTransaction
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(trans, CommandType.Text, "Select * from TableTransaction where ProdId=?", new TurboDBParameter("@prodid", 24));
		/// </remarks>
		/// <param name="transaction">A valid TurboDBTransaction</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <param name="commandParameters">An array of TurboDBParamters used to execute the command</param>
		/// <returns>A dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(TurboDBTransaction transaction, CommandType commandType, string commandText, params TurboDBParameter[] commandParameters)
		{
			if( transaction == null ) throw new ArgumentNullException( "transaction" );
			if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );

			// Create a command and prepare it for execution
			TurboDBCommand cmd = new TurboDBCommand();
			bool mustCloseConnection = false;
            PrepareCommand(cmd, (TurboDBConnection)transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
    			
			// Create the DataAdapter & DataSet
			//using( TurboDBDataAdapter da = new TurboDBDataAdapter(cmd) )
			
			TurboDBDataAdapter da = new TurboDBDataAdapter(cmd);
			
			DataSet ds = new DataSet();
			ds.Locale  =CultureInfo.InvariantCulture;

			// Fill the DataSet using default values for DataTable names, etc
			da.Fill(ds);
    		
			// Detach the TurboDBParameters from the command object, so they can be used again
			cmd.Parameters.Clear();

			// Return the dataset
			return ds;
		
		}
		
		#endregion ExecuteDataset
		
		#region ExecuteReader

		/// <summary>
		/// This enum is used to indicate whether the connection was provided by the caller, or created by TurboDBHelper, so that
		/// we can set the appropriate CommandBehavior when calling ExecuteReader()
		/// </summary>
		private enum TurboDBConnectionOwnership	
		{
			/// <summary>Connection is owned and managed by TurboDBHelper</summary>
			Internal, 
			/// <summary>Connection is owned and managed by the caller</summary>
			External
		}

		/// <summary>
		/// Create and prepare a TurboDBCommand, and call ExecuteReader with the appropriate CommandBehavior.
		/// </summary>
		/// <remarks>
		/// If we created and opened the connection, we want the connection to be closed when the DataReader is closed.
		/// 
		/// If the caller provided the connection, we want to leave it to them to manage.
		/// </remarks>
		/// <param name="connection">A valid TurboDBConnection, on which to execute this command</param>
		/// <param name="transaction">A valid TurboDBTransaction, or 'null'</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <param name="commandParameters">An array of TurboDBParameters to be associated with the command or 'null' if no parameters are required</param>
		/// <param name="connectionOwnership">Indicates whether the connection parameter was provided by the caller, or created by TurboDBHelper</param>
		/// <returns>TurboDBDataReader containing the results of the command</returns>
		private static TurboDBDataReader ExecuteReader(TurboDBConnection connection, TurboDBTransaction transaction, CommandType commandType, string commandText, TurboDBParameter[] commandParameters, TurboDBConnectionOwnership connectionOwnership)
		{	
			if( connection == null ) throw new ArgumentNullException( "connection" );

			bool mustCloseConnection = false;
			// Create a command and prepare it for execution
			TurboDBCommand cmd = new TurboDBCommand();
			try
			{
				PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );
			
				// Create a reader
				TurboDBDataReader dataReader;

				// Call ExecuteReader with the appropriate CommandBehavior
				if (connectionOwnership == TurboDBConnectionOwnership.External)
				{
					dataReader = cmd.ExecuteReader();
				}
				else
				{
					dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				}
			
				// Detach the TurboDBParameters from the command object, so they can be used again.
				// HACK: There is a problem here, the output parameter values are fletched 
				// when the reader is closed, so if the parameters are detached from the command
				// then the TurboDBReader can´t set its values. 
				// When this happen, the parameters can´t be used again in other command.
				bool canClear = true;
				foreach(TurboDBParameter commandParameter in cmd.Parameters)
				{
					if (commandParameter.Direction != ParameterDirection.Input)
						canClear = false;
				}
            
				if (canClear)
				{
					cmd.Parameters.Clear();
				}

				return dataReader;
			}
			catch
			{
				if( mustCloseConnection )
					connection.Close();
				throw;
			}
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns a resultset and takes no parameters) against the database specified in 
		/// the connection string. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  TurboDBDataReader dr = ExecuteReader(connString, CommandType.Text, "Select Orderid from TableTransaction");
		/// </remarks>
		/// <param name="connectionString">A valid connection string for a TurboDBConnection</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <returns>A TurboDBDataReader containing the resultset generated by the command</returns>
		public static TurboDBDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
		{
			// Pass through the call providing null for the set of TurboDBParameters
			return ExecuteReader(connectionString, commandType, commandText, (TurboDBParameter[])null);
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns a resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  TurboDBDataReader dr = ExecuteReader(connString, CommandType.Text, "Select Orderid from TableTransaction where ProdId=?", new TurboDBParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">A valid connection string for a TurboDBConnection</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <param name="commandParameters">An array of TurboDBParamters used to execute the command</param>
		/// <returns>A TurboDBDataReader containing the resultset generated by the command</returns>
		public static TurboDBDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params TurboDBParameter[] commandParameters)
		{
			if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
			TurboDBConnection connection = null;
			try
			{
				connection = new TurboDBConnection(connectionString);

				// Call the private overload that takes an internally owned connection in place of the connection string
				return ExecuteReader(connection, null, commandType, commandText, commandParameters,TurboDBConnectionOwnership.Internal);
			}
			catch
			{
				// If we fail to return the TurboDBDatReader, we need to close the connection ourselves
				if( connection != null ) connection.Close();
				throw;
			}
            
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns a resultset and takes no parameters) against the provided TurboDBConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  TurboDBDataReader dr = ExecuteReader(conn, CommandType.Text, "Select Orderid from TableTransaction");
		/// </remarks>
		/// <param name="connection">A valid TurboDBConnection</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <returns>A TurboDBDataReader containing the resultset generated by the command</returns>
		public static TurboDBDataReader ExecuteReader(TurboDBConnection connection, CommandType commandType, string commandText)
		{
			// Pass through the call providing null for the set of TurboDBParameters
			return ExecuteReader(connection, commandType, commandText, (TurboDBParameter[])null);
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns a resultset) against the specified TurboDBConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  TurboDBDataReader dr = ExecuteReader(conn, CommandType.Text, "Select Orderid from TableTransaction where ProdId=?", new TurboDBParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connection">A valid TurboDBConnection</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <param name="commandParameters">An array of TurboDBParamters used to execute the command</param>
		/// <returns>A TurboDBDataReader containing the resultset generated by the command</returns>
		public static TurboDBDataReader ExecuteReader(TurboDBConnection connection, CommandType commandType, string commandText, params TurboDBParameter[] commandParameters)
		{
			// Pass through the call to the private overload using a null transaction value and an externally owned connection
			return ExecuteReader(connection, (TurboDBTransaction)null, commandType, commandText, commandParameters, TurboDBConnectionOwnership.External);
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns a resultset and takes no parameters) against the provided TurboDBTransaction. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  TurboDBDataReader dr = ExecuteReader(trans, CommandType.Text, "Select Orderid from TableTransaction");
		/// </remarks>
		/// <param name="transaction">A valid TurboDBTransaction</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <returns>A TurboDBDataReader containing the resultset generated by the command</returns>
		public static TurboDBDataReader ExecuteReader(TurboDBTransaction transaction, CommandType commandType, string commandText)
		{
			// Pass through the call providing null for the set of TurboDBParameters
			return ExecuteReader(transaction, commandType, commandText, (TurboDBParameter[])null);
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns a resultset) against the specified TurboDBTransaction
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///   TurboDBDataReader dr = ExecuteReader(trans, CommandType.Text, "Select Orderid from TableTransaction where ProdId=?", new TurboDBParameter("@prodid", 24));
		/// </remarks>
		/// <param name="transaction">A valid TurboDBTransaction</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <param name="commandParameters">An array of TurboDBParamters used to execute the command</param>
		/// <returns>A TurboDBDataReader containing the resultset generated by the command</returns>
		public static TurboDBDataReader ExecuteReader(TurboDBTransaction transaction, CommandType commandType, string commandText, params TurboDBParameter[] commandParameters)
		{
			if( transaction == null ) throw new ArgumentNullException( "transaction" );
			if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );

			// Pass through to private overload, indicating that the connection is owned by the caller
            return ExecuteReader((TurboDBConnection)transaction.Connection, transaction, commandType, commandText, commandParameters, TurboDBConnectionOwnership.External);
		}

		#endregion ExecuteReader

		#region ExecuteScalar
		
		/// <summary>
		/// Execute a TurboDBCommand (that returns a 1x1 resultset and takes no parameters) against the database specified in 
		/// the connection string. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(connString, CommandType.Text, "GetOrderCount");
		/// </remarks>
		/// <param name="connectionString">A valid connection string for a TurboDBConnection</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
		{
			// Pass through the call providing null for the set of TurboDBParameters
			return ExecuteScalar(connectionString, commandType, commandText, (TurboDBParameter[])null);
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns a 1x1 resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(connString, CommandType.Text, "Select count(Order) from TableTransaction where ProdId=?", new TurboDBParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">A valid connection string for a TurboDBConnection</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <param name="commandParameters">An array of TurboDBParamters used to execute the command</param>
		/// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params TurboDBParameter[] commandParameters)
		{
			if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
			// Create & open a TurboDBConnection, and dispose of it after we are done
			using (TurboDBConnection connection = new TurboDBConnection(connectionString))
			{
				// Call the overload that takes a connection in place of the connection string
				return ExecuteScalar(connection, commandType, commandText, commandParameters);
			}
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns a 1x1 resultset and takes no parameters) against the provided TurboDBConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(conn, CommandType.Text, "Select count(Order) from TableTransaction");
		/// </remarks>
		/// <param name="connection">A valid TurboDBConnection</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(TurboDBConnection connection, CommandType commandType, string commandText)
		{
			// Pass through the call providing null for the set of TurboDBParameters
			return ExecuteScalar(connection, commandType, commandText, (TurboDBParameter[])null);
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns a 1x1 resultset) against the specified TurboDBConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(conn, CommandType.Text, "Select count(Order) from TableTransaction where ProdId=?", new TurboDBParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connection">A valid TurboDBConnection</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <param name="commandParameters">An array of TurboDBParamters used to execute the command</param>
		/// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(TurboDBConnection connection, CommandType commandType, string commandText, params TurboDBParameter[] commandParameters)
		{
			if( connection == null ) throw new ArgumentNullException( "connection" );

			// Create a command and prepare it for execution
			TurboDBCommand cmd = new TurboDBCommand();

			bool mustCloseConnection = false;
			PrepareCommand(cmd, connection, (TurboDBTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection );
    			
			// Execute the command & return the results
			object retval = cmd.ExecuteScalar();
    			
			// Detach the TurboDBParameters from the command object, so they can be used again
			cmd.Parameters.Clear();

			if( mustCloseConnection )
				connection.Close();

			return retval;
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns a 1x1 resultset and takes no parameters) against the provided TurboDBTransaction. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(trans, CommandType.Text, "Select count(Order) from TableTransaction");
		/// </remarks>
		/// <param name="transaction">A valid TurboDBTransaction</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(TurboDBTransaction transaction, CommandType commandType, string commandText)
		{
			// Pass through the call providing null for the set of TurboDBParameters
			return ExecuteScalar(transaction, commandType, commandText, (TurboDBParameter[])null);
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns a 1x1 resultset) against the specified TurboDBTransaction
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(trans, CommandType.Text, "Select count(Order) from TableTransaction where ProdId=?", new TurboDBParameter("@prodid", 24));
		/// </remarks>
		/// <param name="transaction">A valid TurboDBTransaction</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <param name="commandParameters">An array of TurboDBParamters used to execute the command</param>
		/// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(TurboDBTransaction transaction, CommandType commandType, string commandText, params TurboDBParameter[] commandParameters)
		{
			if( transaction == null ) throw new ArgumentNullException( "transaction" );
			if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );

			// Create a command and prepare it for execution
			TurboDBCommand cmd = new TurboDBCommand();
			bool mustCloseConnection = false;
            PrepareCommand(cmd, (TurboDBConnection)transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
    			
			// Execute the command & return the results
			object retval = cmd.ExecuteScalar();
    			
			// Detach the TurboDBParameters from the command object, so they can be used again
			cmd.Parameters.Clear();
			return retval;
		}

		#endregion ExecuteScalar	

		#region ExecuteXml
		/// <summary>
		/// Execute a TurboDBCommand (that returns a resultset and takes no parameters) against the provided TurboDBConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  string r = ExecuteXml(conn, CommandType.Text, "Select * from TableTransaction");
		/// </remarks>
		/// <param name="connection">A valid TurboDBConnection</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command using "FOR XML AUTO"</param>
		/// <returns>An string containing the resultset generated by the command</returns>
		public static string ExecuteXml(TurboDBConnection connection, CommandType commandType, string commandText)
		{
			// Pass through the call providing null for the set of TurboDBParameters
			return ExecuteXml(connection, commandType, commandText, (TurboDBParameter[])null);
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns a resultset) against the specified TurboDBConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		/// string  r = ExecuteXml(conn, CommandType.Text, "Select * from TableTransaction where ProdId=?", new TurboDBParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connection">A valid TurboDBConnection</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command using "FOR XML AUTO"</param>
		/// <param name="commandParameters">An array of TurboDBParamters used to execute the command</param>
		/// <returns>An string containing the resultset generated by the command</returns>
		public static string ExecuteXml(TurboDBConnection connection, CommandType commandType, string commandText, params TurboDBParameter[] commandParameters)
		{
			if( connection == null ) throw new ArgumentNullException( "connection" );
			
			bool mustCloseConnection = false;
			// Create a command and prepare it for execution
			TurboDBCommand cmd = new TurboDBCommand();
			try
			{
				PrepareCommand(cmd, connection, (TurboDBTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection );
			
				// Create the DataAdapter & DataSet
				TurboDBDataAdapter obj_Adapter =new TurboDBDataAdapter (cmd);
				DataSet ds=new DataSet();
				ds.Locale  =CultureInfo.InvariantCulture;
				obj_Adapter.Fill(ds); 
							
				// Detach the TurboDBParameters from the command object, so they can be used again
				cmd.Parameters.Clear();
				string retval= ds.GetXml();
				 ds.Clear();
				 obj_Adapter.Dispose ();
				return retval;
				 
			}
			catch
			{	
				if( mustCloseConnection )
					connection.Close();
				throw;
			}
		}

		
		/// <summary>
		/// Execute a TurboDBCommand (that returns a resultset and takes no parameters) against the provided TurboDBTransaction. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		/// string r = ExecuteXmlReader(trans, CommandType.Text, "Select * from TableTransaction");
		/// </remarks>
		/// <param name="transaction">A valid TurboDBTransaction</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command using "FOR XML AUTO"</param>
		/// <returns>An string containing the resultset generated by the command</returns>
		public static string ExecuteXml(TurboDBTransaction transaction, CommandType commandType, string commandText)
		{
			// Pass through the call providing null for the set of TurboDBParameters
			return ExecuteXml(transaction, commandType, commandText, (TurboDBParameter[])null);
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns a resultset) against the specified TurboDBTransaction
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  XmlReader r = ExecuteXmlReader(trans, CommandType.Text, "Select * from TableTransaction where ProdId=?", new TurboDBParameter("@prodid", 24));
		/// </remarks>
		/// <param name="transaction">A valid TurboDBTransaction</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command using "FOR XML AUTO"</param>
		/// <param name="commandParameters">An array of TurboDBParamters used to execute the command</param>
		/// <returns>An XmlReader containing the resultset generated by the command</returns>
		public static string ExecuteXml(TurboDBTransaction transaction, CommandType commandType, string commandText, params TurboDBParameter[] commandParameters)
		{
			if( transaction == null ) throw new ArgumentNullException( "transaction" );
			if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
			
//			// Create a command and prepare it for execution
			TurboDBCommand cmd = new TurboDBCommand();
			bool mustCloseConnection = false;
            PrepareCommand(cmd, (TurboDBConnection)transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
			
			// Create the DataAdapter & DataSet
			TurboDBDataAdapter obj_Adapter =new TurboDBDataAdapter (cmd);
			DataSet ds=new DataSet();
			ds.Locale  =CultureInfo.InvariantCulture;
			obj_Adapter.Fill(ds); 
			
			// Detach the TurboDBParameters from the command object, so they can be used again
			cmd.Parameters.Clear();
			string retval= ds.GetXml();
			ds.Clear();
			obj_Adapter.Dispose ();
			return retval;			
		}

		#endregion ExecuteXml

		#region FillDataset
		/// <summary>
		/// Execute a TurboDBCommand (that returns a resultset and takes no parameters) against the database specified in 
		/// the connection string. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  FillDataset(connString, CommandType.Text, "Select * from TableTransaction", ds, new string[] {"orders"});
		/// </remarks>
		/// <param name="connectionString">A valid connection string for a TurboDBConnection</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
		/// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
		/// by a user defined name (probably the actual table name)</param>
		public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
		{
			if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
			if( dataSet == null ) throw new ArgumentNullException( "dataSet" );
            
			// Create & open a TurboDBConnection, and dispose of it after we are done
			using (TurboDBConnection connection = new TurboDBConnection(connectionString))
			{
				// Call the overload that takes a connection in place of the connection string
				FillDataset(connection, commandType, commandText, dataSet, tableNames);
			}
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns a resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  FillDataset(connString, CommandType.Text, "Select * from TableTransaction where ProdId=?", ds, new string[] {"orders"}, new TurboDBParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">A valid connection string for a TurboDBConnection</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <param name="commandParameters">An array of TurboDBParamters used to execute the command</param>
		/// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
		/// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
		/// by a user defined name (probably the actual table name)
		/// </param>
		public static void FillDataset(string connectionString, CommandType commandType,
			string commandText, DataSet dataSet, string[] tableNames,
			params TurboDBParameter[] commandParameters)
		{
			if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
			if( dataSet == null ) throw new ArgumentNullException( "dataSet" );
			// Create & open a TurboDBConnection, and dispose of it after we are done
			using (TurboDBConnection connection = new TurboDBConnection(connectionString))
			{
				// Call the overload that takes a connection in place of the connection string
				FillDataset(connection, commandType, commandText, dataSet, tableNames, commandParameters);
			}
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns a resultset and takes no parameters) against the provided TurboDBConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  FillDataset(conn, CommandType.Text, "Select * from TableTransaction", ds, new string[] {"orders"});
		/// </remarks>
		/// <param name="connection">A valid TurboDBConnection</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
		/// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
		/// by a user defined name (probably the actual table name)
		/// </param>    
		public static void FillDataset(TurboDBConnection connection, CommandType commandType, 
			string commandText, DataSet dataSet, string[] tableNames)
		{
			FillDataset(connection, commandType, commandText, dataSet, tableNames, null);
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns a resultset) against the specified TurboDBConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  FillDataset(conn, CommandType.Text, "Select * from TableTransaction where ProdId=?", ds, new string[] {"orders"}, new TurboDBParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connection">A valid TurboDBConnection</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
		/// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
		/// by a user defined name (probably the actual table name)
		/// </param>
		/// <param name="commandParameters">An array of TurboDBParamters used to execute the command</param>
		public static void FillDataset(TurboDBConnection connection, CommandType commandType, 
			string commandText, DataSet dataSet, string[] tableNames,
			params TurboDBParameter[] commandParameters)
		{
			FillDataset(connection, null, commandType, commandText, dataSet, tableNames, commandParameters);
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns a resultset and takes no parameters) against the provided TurboDBTransaction. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  FillDataset(trans, CommandType.Text, "Select * from TableTransaction", ds, new string[] {"orders"});
		/// </remarks>
		/// <param name="transaction">A valid TurboDBTransaction</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
		/// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
		/// by a user defined name (probably the actual table name)
		/// </param>
		public static void FillDataset(TurboDBTransaction transaction, CommandType commandType, 
			string commandText,
			DataSet dataSet, string[] tableNames)
		{
			FillDataset (transaction, commandType, commandText, dataSet, tableNames, null);    
		}

		/// <summary>
		/// Execute a TurboDBCommand (that returns a resultset) against the specified TurboDBTransaction
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  FillDataset(trans, CommandType.Text, "Select * from TableTransaction where ProdId=?", ds, new string[] {"orders"}, new TurboDBParameter("@prodid", 24));
		/// </remarks>
		/// <param name="transaction">A valid TurboDBTransaction</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
		/// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
		/// by a user defined name (probably the actual table name)
		/// </param>
		/// <param name="commandParameters">An array of TurboDBParamters used to execute the command</param>
		public static void FillDataset(TurboDBTransaction transaction, CommandType commandType, 
			string commandText, DataSet dataSet, string[] tableNames,
			params TurboDBParameter[] commandParameters)
		{
            FillDataset((TurboDBConnection)transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters);
		}

		/// <summary>
		/// Private helper method that execute a TurboDBCommand (that returns a resultset) against the specified TurboDBTransaction and TurboDBConnection
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  FillDataset(conn, trans, CommandType.Text, "Select * from TableTransaction where ProdId=?", ds, new string[] {"orders"}, new TurboDBParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connection">A valid TurboDBConnection</param>
		/// <param name="transaction">A valid TurboDBTransaction</param>
		/// <param name="commandType">The CommandType (TableDirect, Text)</param>
		/// <param name="commandText">The T-SQL command</param>
		/// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
		/// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
		/// by a user defined name (probably the actual table name)
		/// </param>
		/// <param name="commandParameters">An array of TurboDBParamters used to execute the command</param>
		private static void FillDataset(TurboDBConnection connection, TurboDBTransaction transaction, CommandType commandType, 
			string commandText, DataSet dataSet, string[] tableNames,
			params TurboDBParameter[] commandParameters)
		{
			if( connection == null ) throw new ArgumentNullException( "connection" );
			if( dataSet == null ) throw new ArgumentNullException( "dataSet" );

			// Create a command and prepare it for execution
			TurboDBCommand command = new TurboDBCommand();
			bool mustCloseConnection = false;
			PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );
    			
			// Create the DataAdapter & DataSet
			TurboDBDataAdapter dataAdapter = new TurboDBDataAdapter(command);

			try
			{
				// Add the table mappings specified by the user
				if (tableNames != null && tableNames.Length > 0)
				{
					string tableName = "Table";
					for (int index=0; index < tableNames.Length; index++)
					{
						if( tableNames[index] == null || tableNames[index].Length == 0 ) throw new ArgumentException( "The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames" );
						dataAdapter.TableMappings.Add(tableName, tableNames[index]);
						tableName += (index + 1).ToString();
					}
				}
                
				// Fill the DataSet using default values for DataTable names, etc
				dataAdapter.Fill(dataSet);

				// Detach the TurboDBParameters from the command object, so they can be used again
				command.Parameters.Clear();
				
				if( mustCloseConnection )
					connection.Close();
			}
			finally
			{
				dataAdapter.Dispose();
			}
                
		}	
		
		#endregion
        
		#region UpdateDataset
		/// <summary>
		/// Executes the respective command for each inserted, updated, or deleted row in the DataSet.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  UpdateDataset(conn, insertCommand, deleteCommand, updateCommand, dataSet, "Order");
		/// </remarks>
		/// <param name="insertCommand">A valid transact-SQL statement to insert new records into the data source</param>
		/// <param name="deleteCommand">A valid transact-SQL statement to delete records from the data source</param>
		/// <param name="updateCommand">A valid transact-SQL statement used to update records in the data source</param>
		/// <param name="dataSet">The DataSet used to update the data source</param>
		/// <param name="tableName">The DataTable used to update the data source.</param>
		public static void UpdateDataset(TurboDBCommand insertCommand, TurboDBCommand deleteCommand, TurboDBCommand updateCommand, DataSet dataSet, string tableName)
		{
			if( insertCommand == null ) throw new ArgumentNullException( "insertCommand" );
			if( deleteCommand == null ) throw new ArgumentNullException( "deleteCommand" );
			if( updateCommand == null ) throw new ArgumentNullException( "updateCommand" );
			if( tableName == null || tableName.Length == 0 ) throw new ArgumentNullException( "tableName" ); 

			// Create a TurboDBDataAdapter, and dispose of it after we are done
			TurboDBDataAdapter dataAdapter = new TurboDBDataAdapter();
			try
			{
				// Set the data adapter commands
				dataAdapter.UpdateCommand = updateCommand;
				dataAdapter.InsertCommand = insertCommand;
				dataAdapter.DeleteCommand = deleteCommand;

				// Update the dataset changes in the data source
				dataAdapter.Update (dataSet,tableName); 

				// Commit all the changes made to the DataSet
				dataSet.AcceptChanges();
			}
			catch (TurboDBException E)
			{string strError=E.Message;}
			finally{dataAdapter.Dispose();}
		}
		#endregion

		#region CreateCommand
		
		/// <summary>
		/// Simplify the creation of a TurboDB command object by allowing
		/// a CommandType and Command Text to be provided
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  TurboDBCommand command = CreateCommand(conn, CommandType.Text, "Select * from Customers");
		/// </remarks>
		/// <param name="connection">A valid TurboDBConnection object</param>
		/// <param name="commandType">CommandType (TableDirect, Text)</param>
		/// <param name="commandText">CommandText</param>
		/// <returns>A valid TurboDBCommand object</returns>
		public static TurboDBCommand CreateCommand(TurboDBConnection connection, CommandType commandType, string commandText ) 
		{
			if( connection == null ) throw new ArgumentNullException( "connection" );
			
			if( commandType == CommandType.StoredProcedure ) throw new ArgumentException("Stored Procedures are not supported.");

			// If we receive parameter values, we need to figure out where they go
			if ((commandText == null) && (commandText.Length<= 0)) throw new ArgumentNullException( "Command Text" );
			 
			// Create a TurboDBCommand
			TurboDBCommand cmd = new TurboDBCommand(commandText, connection );
			cmd.CommandType = CommandType.Text ;
			 
			return cmd;
			
		}
		#endregion
	}

}
