		Sybase SQL Anywhere Studio 9.0.2 Read Me First

		     Copyright (c) 1989-2004 Sybase, Inc. 

	 Portions Copyright (c) 2002-2004, iAnywhere Solutions, Inc.

	    All rights reserved. All unpublished rights reserved.

Contents
========

    1. Updated Components and Upgrade Information

    2. New Features in version 9.0.2
    - Adaptive Server Anywhere Databases
    - MobiLink Synchronization
    - UltraLite Databases
    - QAnywhere Messaging

    3. Behavior changes in version 9.0.2
    - Adaptive Server Anywhere Databases
    - MobiLink Synchronization
    - UltraLite Databases
    - QAnywhere Messaging

    4. Known issues and limitations
    - Install issue on Windows 95
    - Native UltraLite for Java and the CrEme VM
    - Notes for users of new PalmOne devices
    - QAnywhere Agent does not work with encrypted communications
    - QAnywhere agent ID on Windows operating systems
    - Additional MobiLink user ID for QAnywhere agents
    - QAnywhere TestMessage sample limitations
    - QAnywhere failover capability
    - Batch file change in ASA samples
    - Upgrading a QAnywhere message store from 9.0.1 to 9.0.2

 *****************************************************************************
 ** For documentation corrections, see the errata file docs\errata_sas.txt, **
 ** accessible from documentation front page.                               **
 *****************************************************************************

1.  Updated Components and Upgrade Note
=======================================

Updated Components
------------------
The following components are included in this version of SQL Anywhere
Studio:

  Adaptive Server Anywhere		Version 9.0.2
  UltraLite 				Version 9.0.2
  MobiLink synchronization		Version 9.0.2
  SQL Remote replication		Version 9.0.2
  InfoMaker				Version 9.0.0 
    (not updated from 9.0.1)
  Open Client/Open Server 		Version 12.5
    (not updated from 9.0.1)
  PowerDesigner PhysicalArchitect	Version 9.5.2
    (not updated from 9.0.1)
  jConnect for JDBC			Version 4.5 and 5.5, EBF 9689
    (not updated from 9.0.1)
  Sybase Central			Version 4.3
   

Upgrade information
-------------------
If you upgrade a version 8 Adaptive Server Anywhere database by unloading and
reloading the database, and if the database is in use as a MobiLink
consolidated database, you must ensure that you use the version 9.0.2 unload
utility (dbunload), not the version 8 unload utility. If you use the version 8
unload utility, the attempt will fail because of schema differences in the
Mobilink catalog tables between 8.x and 9.x databases.  

2.  New features in version 9.0.2
=================================

This section lists the new features introduced in components of SQL Anywhere
Studio version 9.0.2.


2.1.  Adaptive Server Anywhere new features
---------------------------------------------

This section introduces the new features in Adaptive Server Anywhere
version 9.0.2. It provides an exhaustive listing of major and minor new
features, with cross references to locations where each feature is discussed
in detail. 

SQL enhancements
----------------

UNIQUEIDENTIFIER is a native data type      The UNIQUEIDENTIFIER data type is
  now a native data type rather than a domain defined on BINARY(16). As a
  result, Adaptive Server Anywhere automatically carries out type conversions
  as needed, so that the STRTOUUID and UUIDTOSTR conversion functions are not
  needed to handle UNIQUEIDENTIFIER values.

  To use the UNIQUEIDENTIFIER data type in databases created before this
  release, you must upgrade the database file format by unloading and
  reloading the database.

Conflict function for RESOLVE UPDATE triggers    The CONFLICT function can be
  used in conflict resolution triggers to determine if a particular column is
  a source of conflict for an UPDATE being performed on a SQL Remote
  consolidated database.

Procedure profiling enhancements     Profiling information can now be
  filtered per user and per connection by calling using the sa_server_option
  stored procedure.

Remote servers can be tested before they are created or modified      The
  Remote Server Creation wizard in Sybase Central has a Test Connection button
  that allows you to test whether the connection information supplied in the
  remote server definition allows you to successfully connect before the
  remote server is created. The Remote Server property sheet in Sybase Central
  has a Test Connection button that allows you to test whether you can
  successfully connect to a remote server if its properties are changed.

INPUT and OUTPUT statements accept ESCAPES clause    This clause allows you to
  specify that characters are recognized and interpreted as special characters
  by the database server.

WAITFOR can wake up when it receives a message from another
connection      The WAITFOR statement can now wake up when it receives a
  message from another connection using the MESSAGE statement.

ALTER DOMAIN statement      The ALTER DOMAIN statement allows you to rename
  user-defined domains and data types.

NO RESULT SET clause for procedures    Declaring a stored procedure NO RESULT
  SET can be used when external environments need to know that the stored
  procedure does not return a result set.

Column statistics updated during index creation    The CREATE INDEX statement
  now has the side effect that column statistics are updated for the indexed
  columns.

Programming interface enhancements
----------------------------------

PHP Driver    The SQLAnywhere PHP module allows access to Adaptive Server 
  Anywhere databases from the PHP scripting language.

Web service clients    In addition to acting as a web-service provider,
  Adaptive Server Anywhere can now act as a web-service client, making it
  possible to create stored procedures and stored functions that access
  Adaptive Server Anywhere web services, as well as standard web services
  available over the internet.

Multiple web service formats supported    The format of the WSDL file provided
  by a DISH service, as well as that of data payloads returned of part of SOAP
  responses, can now be selected to best suit the needs of the client
  applications. You can now choose between DNET for Microsoft .NET, CONCRETE
  for clients that automatically generate interfaces, and a general-purpose
  XML format.

ODBC_DESCRIBE_BINARY_AS_VARBINARY      This option allows you to choose
  whether you want all BINARY and VARBINARY columns to be described to your
  application as BINARY or VARBINARY.

db_locate_servers_ex    This embedded SQL function provides programmatic
  access to the information displayed by the dblocate -n option, listing all
  the Adaptive Server Anywhere database servers on a specific host.

New PREFETCH option value    The PREFETCH option now has an additional value
  of ALWAYS. This value means that cursor results are prefetched even for
  SENSITIVE cursor types and cursors that involve a proxy table.

Administrative enhancements
---------------------------

SNMP Agent      Adaptive Server Anywhere can now be monitored from Simple
  Network Management Protocol (SNMP) applications.

Deadlock reporting      You can now obtain information about connections
  involved in deadlock using a new database option, LOG_DEADLOCKS, and a new
  system stored procedure, sa_report_deadlocks. When you turn on the
  LOG_DEADLOCKS option, the database server records information about
  deadlocks in an internal buffer. You can obtain deadlock information from
  this internal buffer by calling sa_report_deadlocks.

New collations      The following collations have been added in this release:

1252SWEFIN    has been added to support Swedish and Finnish. On Swedish and
  Finnish systems, the database server will choose 1252SWEFIN as the default
  collation for a new database if no collation is specified.

1255HEB    has been added to support Hebrew. On Hebrew Windows systems, the
  database server will choose 1255HEB as the default collation for a new
  database if no collation is specified.

1256ARA    has been added to support Arabic. On Arabic Windows systems, the
  database server will choose 1256ARA as the default collation for a new
  database if no collation is specified.

950ZHO_HK and 950ZHO_TW     have been added to support Chinese. 950ZHO_HK
  provides support for the Windows Traditional Chinese character set cp950
  plus the Hong Kong Supplementary Character Set (HKSCS). Collation 950ZHO_TW
  provides support for the Windows Traditional Chinese character set cp950,
  but doesn't support HKSCS. Ordering is based on a byte-by-byte ordering of
  the Traditional Chinese characters. These collations supercede the
  deprecated 950TWN collation.

1252SPA    has been added to support Spanish. On Spanish Windows systems, the
  database server will choose 1252SPA as the default collation for a new
  database if a collation is not specified.

874THAIBIN    has been added to support Thai. This is the recommended
  collation for Thai on both Windows and UNIX systems.

New Service Creation (dbsvc) utility options      The Service Creation utility
  (dbsvc) supports the following new options:

  -cm option    This option displays the command used to create the specified
  service. This may be useful for deploying services, or for restoring them to
  their original state.

  -sd option    This option allows you to provide a description of the service,
  which appears in the Windows Service Manager.

  -sn option    This option allows you to provide a name for the service, which
  appears in the Windows Service Manager.

New Data Source (dbdsn) utility options      The Data Source utility (dbdsn)
  supports the following new options:

  -cm option    This option displays the command used to create the specified
  data source. This may be useful for deploying data sources, or for restoring
  them to their original state.

Driver connection parameter      You can use the Driver connection parameter
  to specify a driver for an ODBC data source when creating data sources using
  the Data Source utility (dbdsn) on Windows. On UNIX, if you do not specify
  the Driver connection parameter, the Data Source utility automatically adds
  a Driver entry with the full path of the Adaptive Server Anywhere ODBC
  driver based on the setting of the ASANY9 environment variable.

Disk full callback support      The -fc database server option allows you to
  specify a DLL containing a callback function that can be used to notify
  users, and possibly take corrective action, when a file system full
  condition is encountered.

Validate Database wizard enhancements      When you validate a database using
  the Validate Database wizard in Sybase Central, the wizard indicates the
  current table being validated, as well as the overall progress of the
  validation operation. In addition, for databases with checksums enabled, you
  can validate both tables and checksums at the same time.

Unloading table data in Sybase Central      You can now unload data from one
  or more tables in Sybase Central in one step using the Unload Data dialog.

New columns added to sa_index_density and sa_index_levels      Three new
  columns have been added to the result sets returned by the sa_index_density
  and sa_index_levels stored procedures: TableId, IndexId, and IndexType. If
  you want to revert to the old behavior of these stored procedures, you can
  drop the stored procedure and recreate it with the columns that were
  included in the result set in previous versions of the software.

HISTORY option for BACKUP and RESTORE DATABASE statements      The HISTORY
  option allows you to control whether BACKUP and RESTORE DATABASE operations
  are recorded in the backup.syb file.

Support for integrated logins using Windows user groups      In addition to
  creating integrated logins for individual users on Windows NT/2000/XP, you
  can now create integrated login mappings to user groups on Windows
  NT/2000/XP. It is recommended that you upgrade your database before using
  this feature.

Managing the size of the request log      The -zn database server option
  allows you to specify how many request log files should be retained.

Free pages at the end of the transaction log are removed when the file is
renamed by a backup      Transaction log files are grown in fixed-size
  increments for better performance. When the transaction log is renamed as
  part of a backup, the free pages at the end of the log are removed, which
  helps free up disk space.

Remote server connections can now be explicitly closed    In previous
  releases, connections from Adaptive Server Anywhere to remote servers were
  disconnected only when a user disconnected from Adaptive Server Anywhere.
  You can now explicitly disconnect Adaptive Server Anywhere from a remote
  server using the new CONNECTION CLOSE clause of the ALTER SERVER statement.

Security enhancements
---------------------

.ini files can be obfuscated with dbfhide      The File Hiding utility
  (dbfhide) can now be used to obfuscate the contents of .ini files used by
  Adaptive Server Anywhere and its utilities.

FIPS-certified security      On Solaris, Linux, NetWare, Mac OS X, and all
  supported Windows platforms except Windows CE, you can now use secure
  communication with FIPS 140-2 certified software from Certicom.

  Strong database encryption using FIPS140-2 certified software from Certicom
  is also available on supported 32-bit Windows platforms.

Miscellaneous enhancements
--------------------------

New connection properties      The following connection properties have been
  added:
  - ClientPort
  - LoginTime
  - ServerPort

LOG_DEADLOCKS option      This option allows you to control whether the
  database server logs information about deadlocks in an internal buffer. This
  option can be used with the sa_report_deadlocks procedure to obtain
  information about deadlock.

ROLLBACK_ON_DEADLOCK option      This option allows you to control whether a
  transaction is automatically rolled back if it encounters a deadlock.

TEMP_SPACE_LIMIT_CHECK option      This option allows you to control what
  happens when a connection requests more than its quota of temporary file
  space.

New system stored procedures    Several new system stored procedures have been
  added:
  - sa_rowgenerator procedure      The sa_rowgenerator system procedure is
    provided as an alternative to the RowGenerator table for returning a
    result set with rows between a specified start and end value.

    You can use this procedure for such tasks as generating a result set with
    rows for every value in a range or generating test data for a known number
    of rows in a result set.

  - sa_send_udp stored procedure      This procedure sends a UDP packet to the
    specified address and can be used with MobiLink server-initiated
    synchronization to wake up the Listener utility (dblsn.exe).

  - sa_verify_password stored procedure      This procedure is used by the
    sp_password stored procedure to verify the current user's password.

Maximum cache size on Windows CE      In previous releases of SQL Anywhere
  Studio, the maximum cache size on Windows CE was 32 MB. This limit has been
  removed and the cache size is now limited by the amount of available memory
  on the device.

New database server option for Linux and Solaris      The -ux database server
  option displays the Server Startup Options dialog or Server Messages window
  on Linux and Solaris when you are starting a database server.


2.2.  MobiLink new features
-----------------------------

Following is a list of changes and additions to the software introduced in
version 9.0.2.

New Redirectors    There is a new native Redirector for Apache. In addition,
  there is now an M-Business Anywhere Redirector. Both are available on
  Windows, Solaris and Linux.

Protocols can now be configured to ignore specified hosts    A new option,
  ignore, can be used to specify hosts that should be ignored by the MobiLink
  synchronization server when they connect.

Prevent clients from waiting to synchronize when the MobiLink server is
busy    You can now prevent clients from waiting to synchronize when the
  server is busy.

Version stored in the consolidated database    The SQL Anywhere Studio version
  and build numbers are now stored in the MobiLink system table ml_property.
  For these entries, the component_name is ML, the property_set_name is
  server_info, the property_name is release_version, and the
  property_value is of the form version.build; for example, 9.0.2.1234.

MobiLink synchronization server supports the new uniqueidentifier data
type    The UNIQUEIDENTIFIER data type is now a native data type rather than a
  domain defined on BINARY(16). As a result, MobiLink remote databases now
  automatically carry out type conversions as needed, so that the String to
  UUID and UUID to String conversion functions are not needed to handle
  UNIQUEIDENTIFIER values.

Security enhancements
---------------------

FIPS-certified security streams    On Windows devices, you can now use secure
  communication with FIPS 140-2 certified software from Certicom.

Connection options now shown in output log    MobiLink now displays the
  connection string and options in the output log, with passwords replaced
  with asterisks.

Deprecated security features    See MobiLink behavior changes.

MobiLink client enhancements
----------------------------

New synchronization setup tool for UltraLite    The UltraLite Schema Painter
  can now generate MobiLink synchronization scripts, as well as database
  tables and triggers for Adaptive Server Anywhere consolidated databases.

  See The UltraLite Schema Painter and Tutorial: Working with UltraLite
  Databases.

Now easier to delete a remote database and recreate it    The first
  synchronization of an Adaptive Server Anywhere client subscription now
  always works.

New dbmlsync hook is called when connections to MobiLink fail    A new event
  hook has been added, sp_hook_dbmlsync_connect_failed, that allows you to
  program ways to recover from failed synchronization connections.

Improved integration of MobiLink clients into HTTP infrastructure    You can
  now synchronize using HTTP when a proxy and/or web server requires RFC 2617
  Basic or Digest authentication.

In addition, two new client connection parameters allow you to specify custom
  HTTP headers and custom cookies. In order to respect session cookies, HTTP
  clients now recognize all Set-Cookie and Set-Cookie2 HTTP headers that they
  receive in server replies and will send these cookies back up with all
  future HTTP requests. If the name of a cookie matches an existing cookie,
  the client will replace its old value with the new one. Cookies are not
  remembered between synchronizations: they are discarded at the end of the
  synchronization.

Assistance in detecting connection errors    MobiLink clients now issue a
  warning message when invalid connection parameters are specified.

Mirror log location    When dbmlsync is run on a different machine from the
  remote database, or when mirror logs are located in a different directory
  from mirror transaction logs, dbmlsync is now able to automatically delete
  old log files when you specify the location of old mirror logs using this
  new extended option.

Server-initiated synchronization enhancements
---------------------------------------------

Enhanced functionality for connection-initiated synchronization    In addition
  to BEST_IP_CHANGED, Windows Listeners now also generate the internal
  message IP_CHANGED to help you initiate synchronization when there is a
  change in connectivity.

Listener post action enhancements    When you specify Listener post actions,
  you can now optionally use a Windows message ID to specify the window
  message, and can optionally use the window title instead of the window
  class. You can also use single quotes around the window class name or
  message if your message or title include non-alphanumeric characters such as
  spaces or punctuation marks.

New action variables    There are several new action variables:
  -  $request_id
  -  $best_ip
  -  $best_adapter_name
  -  $best_adapter_mac
  -  $best_network_name

More device support    The Palm Listener now supports Kyocera 7135 and Treo
  600 smartphones.


2.3.  SQL Remote new features
-------------------------------

Following is a list of changes and additions to the software introduced in
version 9.0.2.

Mirror log location      When dbremote is run on a different machine from the
  remote database, or when mirror logs are located in a different directory
  from mirror transaction logs, dbremote can automatically delete old log
  files if you specify the location of old mirror logs using the new -ml
  option.

Conflict function for RESOLVE UPDATE triggers    The CONFLICT function can be
  used in conflict resolution triggers to determine if a particular column is
  a source of conflict for an UPDATE being performed on a SQL Remote
  consolidated database.


2.4.  UltraLite new features
------------------------------

Following is a list of changes and additions to the software introduced in
version 9.0.2.

Component new features
----------------------

ADO.NET interface in UltraLite.NET      UltraLite.NET now supports the ADO.NET
  programming interface in the new namespace iAnywhere.Data.UltraLite. ADO.NET
  provides an industry-standard interface to UltraLite, and also provides an
  easy migration path to Adaptive Server Anywhere for large applications.

  The ADO.NET interface is recommended over the previous UltraLite.NET
  interface (iAnywhere.UltraLite namespace), which is now deprecated.

UltraLite for MobileVB enhancements      UltraLite for MobileVB now supports
  Visual Basic .NET programming using AppForge Crossfire.

UltraLite for M-Business Anywhere enhancements       The following
  enhancements have been made to UltraLite for M-Business Anywhere:

UltraLite for M-Business Anywhere now supports the client/server UltraLite
  engine. Your application can use the DatabaseManager.runtimeType property to
  inspect whether the engine or the runtime library is being used.

UltraLite for M-Business Anywhere applications can now synchronize both data
  and web content with a single operation.

You can use a MobiLink Redirector to synchronize both data and web content
  through a single M-Business Anywhere server. For synchronization from
  outside firewalls, this reduces the number of ports that need to be
  accessible.

M-Business Anywhere 5.5 on Windows XP is now a supported platform. The
  connection parameters databaseOnDesktop and schemaOnDesktop support this
  environment.

Additional methods have been added to the API that enable you to gather
  information about data using the column ID rather than the column name.

Native UltraLite for Java enhancements      The following enhancements have
  been made to Native UltraLite for Java:

  - schema info is accessible by columnID instead of just name.

  - SyncProgressData ErrorMessage property has been added, and provides
    improved sync error reporting.

  - The ResultSetSchema now keeps PreparedStatement alive while in use.

UltraLite.NET component enhancements    The following functions are supported
  by UltraLite.NET. It is recommended that these functions be used as part of
  the ADO.NET interface (iAnywhere.Data.UltraLite namespace).

  -  New ULCursorSchema.Name, ULResultSetSchema.Name read-only properties.

  -  New ULSyncProgressData ErrorMessage property and improved sync error
  reporting.

  -  ULCommand.Plan read-only property.

Palm developers can now use a version-independent prefix file      In previous
  releases, the UltraLite prefix file depended on the version of Palm OS for
  which you were developing. You can now use ulpalmos.h for any version of
  Palm OS.

Palm developers can now use expanded mode      CodeWarrior supports a code
  generation mode called expanded mode    , which improves memory use for
  global data. You can now use an expanded mode version of the UltraLite
  runtime library.

Trusted certificates can be retrieved from permanent storage    In previous
  releases of the software, the trusted certificate for secure synchronization
  was embedded in the database schema. On Windows and Windows CE platforms, it
  can now be stored externally and accessed via the trusted_certificates
  option.

SQL and runtime enhancements
----------------------------

Dynamic SQL enhancements    The following enhancements have been made to the
  UltraLite dynamic SQL support:

Query optimization improvement      In previous versions of the software, the
  order in which tables were accessed was the order in which they appeared in
  the query. In this version, UltraLite optimizes the query to find an
  efficient order in which to access tables. As long as you have defined
  appropriate indexes in the database, the optimizer helps to improve query
  execution performance.

Query plan viewing    Query access plans now include the index name instead of
  an index number, for clarity. Access plans can be seen, for example, from
  the new UltraLite Interactive SQL utility.

IF and CASE expressions      The range of expressions supported by UltraLite
  has been extended by adding these two conditional expressions. 

Table names can have owner names      UltraLite tables do not have owners.
  Support has been added for owner.table-name as a convenience for existing
  SQL and for programmatically-generated SQL. UltraLite accepts but ignores
  owner.

UNIQUEIDENTIFIER data type introduced    The UNIQUEIDENTIFIER data type is now
  a native data type rather than a domain defined on BINARY(16). As a result,
  UltraLite automatically carries out type conversions as needed, so that the
  String to UUID and UUID to String conversion functions are not needed to
  handle UNIQUEIDENTIFIER values.

UltraLite query plan descriptions enhanced    UltraLite query plan
  descriptions, which can be viewed in UltraLite Interactive SQL, have been
  enhanced to be easier to read for better diagnosis of performance issues.

Administration enhancements
---------------------------

UltraLite Interactive SQL utility      An UltraLite Interactive SQL utility is
  now provided for testing SQL statements against UltraLite databases and for
  modifying UltraLite data. It also displays query plans so that you can
  diagnose performance problems.

Command line utilities for database management     A set of command line
  utilities makes database management tasks easier for UltraLite files on
  Windows machines. These utilities are particularly useful during application
  development.

  Each of the new utilities carries out a subset of the tasks that the ulconv
  utility provides. In future versions of the software, the ulconv utility
  will be replaced by these newer single-task utilities.
  - The ulcreate utility
  - The ulload utility
  - The ulsync utility
  - The ulunload utility

Synchronization enhancements
----------------------------

Improved integration of MobiLink clients into HTTP infrastructure    Two new
  client connection parameters allow you to specify custom headers and custom
  cookies.

Synchronization script generation from the Schema Painter      The UltraLite
  Schema Painter now provides the ability to generate synchronization scripts
  for Adaptive Server Anywhere consolidated databases. This capability makes
  it easier to extend UltraLite applications to a synchronized architecture.

Synchronization notifications on referential integrity violations      
  A new callback function or event (depending on the interface) has been added 
  to allow reporting of referential integrity violations on synchronization.

2.5.  QAnywhere new features
------------------------------

Failover servers    The QAnywhere agent now can take a list of MobiLink server
  connection protocol options rather than just one. 

Emulator support    QAnywhere client applications on the Pocket PC 2002 and
  Pocket PC 2003 now support x86 emulators. Only "scheduled" policy for the
  QAnywhere Agent is supported on these emulators.

New RDBMSs supported as server message stores    All supported MobiLink
  consolidated databases can now be used in QAnywhere applications as server
  message stores: Adaptive Server Anywhere, Adaptive Server Enterprise,
  Microsoft SQL Server, Oracle, and DB2.

QAnywhere .NET client library for the .NET Compact Framework now supports
message listeners.     QAnywhere .NET client library for the .NET Compact
  Framework now supports message listeners.

Transmission rule enhancements
------------------------------

Remote message store properties now synchronized    When you set remote
  message store properties, those properties are now synchronized to the
  server message store so that they can be used in transmission rules.

Enhanced message store properties    The ias_Network property now contains
  fields you can use to access detailed network information.

  In addition, you can now create customized message store properties.

Rules for deleting messages    You can now specify transmission rules for the
  persistence of messages in the message stores. You can delete messages on
  the client side and server side.

QAnywhere Agent enhancements
----------------------------

Connection string    To start the local message store, you can now specify a
  connection string with the qaagent -c option. This allows you to use
  Adaptive Server Anywhere connection string parameters.

Quiet mode    The QAnywhere Agent now supports two flavors of quiet mode,
  which can avoid problems on some Windows CE devices.

QAstop utility      When you start the QAnywhere Agent in quiet mode with the
  -qi option, you must use the new qastop utility to stop it.

Enhanced verbosity    You can now specify output log file names with the -o or
  -ot option, and regulate the size of the output files with the -on, -os and
  -ot options. In addition, the -v option replaces the old -verbose option.
  With -v, you have greater control over logging output.

Initialize database for use as a remote message store    You can use the new
  qaagent -si option to set up a remote message store. For more information,
  see -si option.

Upgrade from version 9.0.1    The QAnywhere Agent has a new option, -su, that
  upgrades a remote message store from version 9.0.1 to 9.0.2.

QAnywhere MobiLink system tables
--------------------------------

All QAnywhere MobiLink system tables are now owned by ml_qa_user_group.
Previously, they were owned by DBO.

Two new MobiLink system tables have been added. For more information, see:

o ml_qa_delivery
o ml_qa_delivery_client

There are changes to the schema of several MobiLink system tables. For more
information, see:

o ml_qa_global_props
o ml_qa_global_props_client
o ml_qa_repository
o ml_qa_repository_client
o ml_qa_repository_props
o ml_qa_repository_props_client

The following MobiLink system tables are not generated for 9.0.2 clients:

o ml_qa_repository_staging_client
o ml_qa_status_staging_client

The following MobiLink system table is not generated for 9.0.2 servers:

o ml_qa_repository_content



2.6.  Documentation enhancements
----------------------------------

This section introduces enhancements made to the appearance, organization, or
navigation of the Adaptive Server Anywhere documentation for version 9.0.2. It
provides an exhaustive listing of major changes.

New documentation
-----------------

The documentation for existing features has been enhanced in several areas,
including the following:

SNMP Agent documentation    A new book has been added that describes the
  Adaptive Server Anywhere SNMP Agent.

Windows CE starting points    A chapter containing starting points for Windows
  CE users has been added.

DBTools interface to the MobiLink synchronization client    A sample and other
  information about how to use dbmlsync from DBTools has been added.

QAnywhere enhancements    The QAnywhere documentation has been expanded, with
  new information about how to integrate messaging with JMS messaging systems
  and MobiLink data synchronization, and enhanced information about setting up
  QAnywhere applications.

Server-initiated synchronization SDKs    The documentation for the SDKs has
  been expanded, and a new section on the Palm Listener SDK has been added.

Documentation enhancements
--------------------------

MobiLink reorganization    The MobiLink books have been reorganized so that
  there is now a client guide, an administration guide, and a book of
  tutorials. As well as covering Adaptive Server Anywhere clients, the client
  guide includes synchronization parameters and synchronization connection
  parameters for UltraLite clients, which were previously in the UltraLite
  Database User's Guide.

UltraLite API and QAnywhere API references    The UltraLite.NET, UltraLite C++
  API, QAnywhere .NET, and QAnywhere C++ API material is now available in the
  same form as the remainder of the documentation. As a result, it is
  available as PDF as well as in the HTML-based documentation.


3.  Behavior changes in version 9.0.2
=======================================

This section lists the behavior changes introduced in components of
SQL Anywhere Studio version 9.0.2. It also lists deprecated features, which
are supported in the current software but will not be supported in the next
major release of SQL Anywhere Studio. 

Deprecated feature list subject to change
-----------------------------------------
As with all forward-looking statements, the list of deprecated features is not
guaranteed to be complete and is subject to change.

3.1  Adaptive Server Anywhere behavior changes
-------------------------------------------------

Deprecated and discontinued features
------------------------------------

The following is a list of features that are no longer supported or are
deprecated, and that may impact existing applications.

MIN_TABLE_SIZE_FOR_HISTOGRAM option removed      The database server no longer
  uses the MIN_TABLE_SIZE_FOR_HISTOGRAM option. In previous versions of the
  software, this option allowed you to specify the minimum table size for
  which histograms were created. Now Adaptive Server Anywhere automatically
  creates histograms for all tables with five or more rows. You can create
  histograms for all tables, regardless of size, using the CREATE STATISTICS
  statement.

Deprecated database options      The following database options are no longer
  supported:
  - TRUNCATE_DATE_VALUES
  - ASSUME_DISTINCT_SERVERS

Old database formats deprecated      In the next major release of SQL Anywhere
  Studio, databases created under old versions of the software will not be
  supported. Migration tools will be provided.

Non-threaded DBTools library for UNIX deprecated      The non-threaded DBTools
  library for UNIX (libdbtool9.so) is deprecated: it is fully supported in the
  current software but will not be supported in the next major release of SQL
  Anywhere Studio.

950TWN collation no longer supported      The 950TWN has been superceded by
  the following collations: 950ZHO_HK and 950ZHO_TW.

Other behavior changes
----------------------

The following is a list of behavior changes from previous versions of the
software.

Restrictions on the Transaction Log utility (dblog) when removing the
transaction log      When removing a transaction log using the -n option, you
  must also specify the corresponding ignore transaction log offset option
  (-il for the Log Transfer Manager, -ir for SQL Remote, or -is for dbmlsync).

Database utilities running in quiet mode      When performing any of the
  following operations with the -q option (quiet mode) specified, you must
  also specify the -y option:

  - modifying or deleting a service with the Service Creation (dbsvc) utility

  modifying or deleting a datasource with the Data Source (dbdsn) utility

  - erasing a file with the Erase (dberase) utility

  - translating a transaction log with the Log Translation (dbtran) utility

Certificate name and password must be supplied when using ECC_TLS or RSA
encryption      The default values for the certificate, certificate_password,
  and trusted_certificates parameters have been removed. These defaults
  utilized the sample certificates that are provided in the win32 directory of
  your SQL Anywhere Studio installation. The sample certificates are useful
  only for testing and development purposes and do not provide security.

  In addition, the -ec all server option is no longer supported.

-xs server option change      The -xs all server option is no longer
  supported to listen for web requests on both HTTP and HTTPS ports.

TCP/IP port number must be specified for network database servers on Mac OS X,
HP-UX, and Tru64 when the default port is not in use    If you are starting a
  database server on Mac OS X, HP-UX, or Tru64, you must specify a port number
  using the ServerPort [PORT] protocol option if the default port (2638) is
  already in use or if you do not wish to use the default port.

Dbspace file names are changed for databases unloaded and reloaded with the
Unload utility (dbunload)      When a database is unloaded and reloaded using
  the -an option of the Unload utility (dbunload), the dbspace file names for
  the new database have an R appended to the end of the file name. This is
  done to prevent naming conflicts when the new dbspace files are placed in
  the same directory as the original dbspace files. Dbspace file names also
  have an R appended to the file name when you unload and reload data using
  the Unload Database wizard in Sybase Central.

Property functions now return LONG VARCHAR values      Previously, the
  following functions returned a VARCHAR(254) value. They now return a
  VARCHAR(maxpropsize) value, where maxpropsize is based on the maximum page
  size specified for the server.
  - CONNECTION_PROPERTY
  - DB_EXTENDED_PROPERTY
  - DB_PROPERTY
  - EVENT_PARAMETER
  - PROPERTY

STRTOUUID function change      In previous releases, if STRTOUUID was passed
  an invalid UUID value it returned NULL. It now returns a conversion error
  unless the CONVERSION_ERROR option is set to OFF, in which case it returns
  NULL.



3.2.  MobiLink behavior changes
---------------------------------

The following is a list of behavior changes from previous versions of the
software.

Security behavior changes
-------------------------

HTTP+TLS security deprecated in favor of HTTPS    Transport-layer security is
  deprecated for clients connecting over HTTP. To use transport-layer security
  over HTTP, you should use HTTPS.

Certificate name and password must be supplied when using ECC_TLS or RSA
encryption with MobiLink    The default values for the certificate,
  certificate_password, and trusted_certificates synchronization parameters
  have been removed. These defaults utilized the sample certificates that are
  provided in the win32 directory of your SQL Anywhere Studio installation.
  The sample certificates are useful only for testing and development purposes
  and do not provide security.

Other MobiLink behavior changes
-------------------------------

No polling interval for UDP listening    On the Listener, there is now no
  polling interval for UDP connections. The Listener processes messages
  immediately.

Support for MobiLink Palm Listener on Treo 180 and Kyocera 6035 smartphones
deprecated.


3.3.  SQL Remote behavior changes
-----------------------------------

Deprecated and discontinued features
------------------------------------

The following is a list of features that are no longer supported or are
deprecated, and that may impact existing applications.

SQL Remote for Adaptive Server Enterprise deprecated      In the next major
  release of SQL Anywhere Studio, SQL Remote for Adaptive Server Enterprise
  will not be present. MobiLink provides a more flexible and scalable solution
  for data synchronization between Adaptive Server Enterprise and Adaptive
  Server Anywhere databases.

Other behavior changes
----------------------

The following is a list of behavior changes from previous versions of the
software.

The Extraction (dbxtract) utility      When extracting a remote database with
  dbxtract, if the -q option (quiet mode) is specified, you should also
  specify the -y option so that dbxtract will automatically replace the
  existing command file without confirmation.

IPM_Receive message control parameter      The default value for the MAPI
  IPM_Receive message control parameter has been changed to YES. Setting this
  value to YES ensures that both IPC and IPM messages are picked up by SQL
  Remote.


3.4.  UltraLite behavior changes
----------------------------------

The next major release of UltraLite will enhance development using industry
standard APIs and will enhance development using the component model as
opposed to the original static interfaces. These changes will have several
benefits for users, including making it easier to develop applications using
UltraLite. 

As a result of these plans, several UltraLite APIs are deprecated with this
release, meaning that they continue to be fully supported in the current
software but will not be supported in the next major release. Assistance in
migrating applications that use deprecated interfaces will be provided in the
next major release.

As with all forward-looking statements, the list of deprecated and
discontinued features provided here is subject to change.

Deprecated and discontinued features
------------------------------------

The following features are deprecated or discontinued.

Static interfaces deprecated      The next major release of SQL Anywhere
  Studio will not support the static C++ API or the static Java API. An
  embedded SQL interface will be available, but not through the current
  generated code mechanism.

UltraLite.NET component interface to be superseded by ADO.NET      In this
  release, UltraLite.NET supports ADO.NET development in the new
  iAnywhere.Data.UltraLite namespace. ADO.NET provides the benefits of an
  industry standard interface and of an easy migration path to Adaptive Server
  Anywhere for large applications. The UltraLite.NET component API
  (iAnywhere.UltraLite namespace) is deprecated in this release and will not
  be provided in the next major release.

Native UltraLite for Java component interface to be superseded by JDBC    The
  current Native UltraLite for Java interface is scheduled to be superseded by
  a JDBC interface.

Other behavior changes
----------------------

The following is a list of behavior changes from previous versions of the
software.

New warning for referential integrity deletes during download      UltraLite
  automatically deletes rows as needed to maintain referential integrity
  during download. It now raises a warning for each row deleted in this way. 

Native UltraLite for Java behavior changes    Cursor.getRowCount() method has
  been changed to return an int. No application changes are required.

UltraLite.NET component behavior changes    Cursor.getRowCount() method has
  been changed to return an int. No application changes are required.

Handling invalid synchronization parameters      In previous releases, the
  UltraLite runtime ignored all invalid synchronization parameters. Misspelled
  parameters were therefore ignored and a default value used instead. 

  In this release, if the runtime encounters an invalid parameter,
  synchronization fails and the SQL code SQLE_UNRECOGNIZED_OPTION is set. If
  an error callback has been provided, it will be called once for each invalid
  parameter. Duplicates continue to be ignored.

New libraries for secure synchronization      The security options for
  synchronization have been moved into separate libraries. If you use either
  of the ULSecureCerticomTLSStream or ULSecureRSATLSStream security options
  for encrypted synchronization, you must now link separately against a
  corresponding static library, or ship a separate DLL.


3.5.  QAnywhere behavior changes
----------------------------------

Deprecated and discontinued features
------------------------------------

QAnywhere Agent options    The following QAnywhere Agent (qaagent) options
  have been deprecated and replaced.

Deprecated qaagent option...Replaced with qaagent option...
  -agent_id id              -id id
  -dbauser user             -c "UID=user"
  -dbeng name               -c "ENG=name"
  -dbfile filename          -c "DBF=filename"
  -dbname name              -c "DBN=name"
  -ek key                   -c "DBKEY=key"
  -password password        -c "PWD=password"
  -sv                       -c "Start={ dbeng9 | dbsrv9 }"
  -verbose                  -v[levels]
In addition, the following qaagent options are no longer required and have
  been deprecated:
  - -e
  - -rb

QAnywhere Agent no longer creates the client message store    You must now
  create the message store database yourself before running qaagent. There is
  a new option, -si, that initializes the database with system objects that
  are required by QAnywhere.

Other behavior changes
----------------------

QAnywhere client library message overhead is reduced    The ias_MessageType
  property is no longer set for regular messages. It is still set for network
  status and other system messages that are sent to the system queue.


4. Known Issues and Limitations
===============================

Install issue on Windows 95
---------------------------
When the documentation installer is launched by the software installer on
Windows 95, it may not receive focus and so may be hidden by the main install
window.  To access the documentation installer, press Alt+Tab.


Native UltraLite for Java and the CrEme VM
------------------------------------------
The SyncProgressDialog, ActiveSyncListener
(DatabaseManager.setActiveSyncListener()), and ServerSyncListener
(DatabaseManager.setServerSyncListener()) do not work with version 3.25 of
NSICom's CrEme VM.

Notes for users of new NVFS flash devices
-----------------------------------------
Some new palmOne NVFS flash devices have built-in memory assigned as VFS
space, while others require an SD card. To use UltraLite with these devices,
you must use the VFS as your data store: that is, you must call
ULEnableFilDB() in your application.  ULEnablePalmRecordDB() is not supported
on these devices.

QAnywhere Agent does not work with encrypted communications
-----------------------------------------------------------
Currently you cannot start the QAnywhere Agent with push notifications enabled
(the default) and a secure stream. We expect to address this issue in an
upcoming EBF.

QAnywhere agent ID on Windows operating systems
-----------------------------------------------
On Windows operating systems other than Windows CE the QAnywhere Agent obtains
an agent ID from the COMPUTERNAME environment variable if no agent ID is
specified on the command line. You must set an agent ID either on the
QAnywhere Agent command line or by ensuring the COMPUTERNAME environment
variable is set before starting the QAnywhere Agent.

Additional MobiLink user ID for QAnywhere agents
------------------------------------------------
If you run the QAnywhere Agent with -push_notifications "enabled", it requires
the existence of an additional MobiLink user whose name is the same as the
agent ID with "_lsn" appended.

If the MobiLink server is started with the -zu+ command line switch, then the
additional user is added automatically, and requires no additional
admininstration.  Otherwise, you must add the user name to the consolidated
database just like the agent ID MobiLink user.  The QAnywhere Agent assumes
the user names for the agent ID and the agent ID "_lsn" share the same
password.

QAnywhere TestMessage sample limitations
----------------------------------------
The MFC version of the TestMessage sample does not run properly on Windows
98 and Windows Me operating systems.  The MFC version of TestMessage is
invoked when you choose "Programs->SQL Anywhere 9->QAnywhere->TestMessage
Sample Application" is selected from the start menu.  
   
To run a TestMessage sample application on Windows 98 and Windows Me, build
and run the .NET version of the application.  For instructions, see the
chapter entitled "Tutorial: A Sample QAnywhere Application" in the QAnywhere
User's Guide.

QAnywhere failover capability
------------------------------
The failover capability of QAnywhere Agent does not work when all of the
following conditions are true:
- the agent is running on a cradled Windows CE device,
- the agent is configured to use TCP/IP for communication with the primary
  MobiLink server,
- the device is using ActiveSync for TCP/IP connections.  
Because of the way TCP/IP is implemented for ActiveSync, the QAnywhere Agent
believes that the TCP/IP connection to the primary MobiLink server always
succeeds, even when the server is unavailable. This results in the failover
MobiLink server never being used.
	
Failover works correctly in the case of a cradled CE device with an
ethernet connection, when ActiveSync is not involved.

Batch file change in ASA samples
--------------------------------
If the "run.bat" batch file fails when running one of the samples in the
Samples\ASA directory, comment out (using REM) the instruction to run dbspawn,
and comment out the instruction to stop the server using dbstop.

Upgrading a QAnywhere message store from 9.0.1 to 9.0.2
-------------------------------------------------------
There have been schema changes to the QAnywhere system tables between 9.0.1
and 9.0.2. These changes have the following consequences:

- When upgrading a 9.0.1 QAnywhere server or client message store to 9.0.2
  using the unload utility, any messages contained in the message store will
  not be copied to the upgraded database.  This issue will be fixed in an
  upcoming EBF.

- When upgrading a Microsoft SQL Server or Adaptive Server Enterprise
  consolidated database server using the MobiLink upgrade scripts from 9.0.1
  to 9.0.2 any QAnywhere message data will be lost. This issue will be fixed
  in an upcoming EBF.

- If you install the 9.0.2 patch on top of a 9.0.1 installation, the shortcut
  to start QAnywhere Agent will not work properly. To solve the problem,
  upgrade qanyserv.db and either upgrade the qanywhere.db sample database or
  create a new one.

  To upgrade qanyserv.db, open a command prompt at the
  %ASANY9%\Samples\QAnywhere\server directory, and execute the following
  command:  
      dbupgrad -c "DBF=qanyserv.db;uid=dba;pwd=sql"

  To upgrade qanywhere.db, execute the following command:  
      qaagent -su -c "DBF=qanywhere.db;uid=dba;pwd=sql".  

  For Windows CE, run this on the desktop and copy the upgraded database to
  your Windows CE device.  Alternatively, to create a new qanywhere.db execute
  the following commands and copy the database to %ASANY9% on your Windows
  machine:
    dbinit -i qanywhere.db
    qaagent -si -c "DBF=qanywhere.db"

  To complete the operation, erase the old database and log file, and copy the
  new qanywhere.db to %ASANY9%\Samples\QAnywhere\Client.  On Windows CE, run
  the Deploy SQL Anywhere for CE shortcut.
