# KuboEstudio.EF
C# Library that allows you to call any database allowed by the EnterpriseLibrary in different ways, creating a common EntityFramework like library for multiple Databases. Also Includes a wrapper for Azure Tables.

# Dll:
  * <b>KuboEstudio.Common.dll</b> (https://www.nuget.org/packages/KuboEstudio.EF.Common): <br />
    This dll contains the main classes and is for being use in the project that defined and use the entities.
  * <b>KuboEstudio.Data.dll</b> (https://www.nuget.org/packages/KuboEstudio.EF.Data): <br />
    This dll contains the classes necessary to access and manipulate any database and use the methods for Function, Query, StoreProcedure, Repository.
  * <b>KuboEstudio.Data.Azure.dll</b> (https://www.nuget.org/packages/KuboEstudio.EF.Data.Azure): <br />
    This dll contains the classes necessary to access and manipulate Azure Tables with the AzureRepository.<br /><br />

# Quick Examples of use:

<b>Functions</b> (<b>KuboEstudio.EF.Resources.Function</b>)<b>:</b><br />
Description: Execute a specific function and gets the result.<br />
Methods:<br />
  * <b>Select:</b> Execute a table function and transforms the result to a List of the specified class entity.<br />
    Ex. <b>List</b><<b>MyClass</b>> results = <b>Function</b>.Select<<b>MyClass</b>>("FunctionName", new object[] { 1, "Val" });
  * <b>SelectSingle:</b> Execute a table function and transforms only the first record of the result to the specified class entity.<br />
    Ex. <b>MyClass</b> result = <b>Function</b>.SelectSingle<<b>MyClass</b>>("TableFunctionName", new object[] { 1, "Val" });
  * <b>SelectTable:</b> Execute a table function and returns the <b>DataTable</b> result.<br />
    Ex. <b>DataTable</b> results = <b>Function</b>.SelectTable("TableFunctionName", new object[] { 1, "Val" });
  * <b>SelectScalar:</b> Execute a scalar function and returns the result.<br />
    Ex. <b>string</b> result = (<b>string</b>)<b>Function</b>.SelectScalar("ScalarFunctionName", new object[] { 1, "Val" });<br />
   &emsp;&nbsp;&nbsp; <b>string</b> result = <b>Function</b>.SelectScalar<<b>string</b>>("ScalarFunctionName", new object[] { 1, "Val" });<br />

<b>Queries</b> (<b>KuboEstudio.EF.Resources.Query</b>)<b>:</b><br />
Description: Execute a user specific query.<br />
Methods:<br />
  * <b>Execute:</b> Execute a query and transforms the result to a List of the specified class entity.<br />
    Ex. <b>List</b><<b>MyClass</b>> results = <b>Query</b>.Execute<<b>MyClass</b>>("SELECT * FROM table WHERE Param = @Param1", <br />
	&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;new <b>Parameter</b>("@Param1", "Value"));
  * <b>ExecuteSingle:</b> Execute a query and transforms only the first record of the result to the specified class entity.<br />
    Ex. <b>MyClass</b> result = <b>Query</b>.ExecuteSingle<<b>MyClass</b>>("SELECT * FROM table WHERE Param = @Param1", <br />
	&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;new <b>Parameter</b>("@Param1", "Value", DbType.String));
  * <b>ExecuteDataSet:</b> Execute a query and returns the <b>DataSet</b> result with multiple tables.<br />
    Ex. <b>DataSet</b> results = <b>Query</b>.ExecuteDataSet("SELECT * FROM table1; SELECT * FROM table2;");
  * <b>ExecuteDataTable:</b> Execute a query and returns the <b>DataTable</b> result.<br />
    Ex. <b>DataTable</b> results = <b>Query</b>.ExecuteDataTable("SELECT * FROM table1" });
  * <b>ExecuteDictionary:</b> Execute a query and get a <b>Dictionary</b> specified with col0 as key and col1 as value.<br />
    Ex. <b>Dictionary</b><int,string> results = <b>Query</b>.ExecuteDictionary<<b>Dictionary</b><int,string>>("SELECT ID, Value FROM table1");
  * <b>ExecuteScalar:</b> Execute a query and get the scalar value.<br />
    Ex. <b>string</b> result = (<b>string</b>)<b>Query</b>.ExecuteScalar("SELECT Top(1) Value FROM table1");<br />
   &emsp;&nbsp;&nbsp; <b>string</b> result = <b>Query</b>.ExecuteScalar<<b>string</b>>("SELECT Top(1) Value FROM table1");
  * <b>ExecuteNonQuery:</b> Execute a query.<br />
    Ex. <b>Query</b>.ExecuteNonQuery("UPDATE Value = @Val FROM table1 WHERE ID = @ID", <br />
	&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;new <b>Parameter</b>("@Description", "Value", DbType.String),<br />
	&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;new <b>Parameter</b>("ID", 1, DbType.Int32));<br />

<b>Store Procedures</b> (<b>KuboEstudio.EF.Resources.StoreProcedure</b>)<b>:</b><br />
Description: Execute a specific store procedure and gets the result.<br />
Methods:<br />
  * <b>Execute:</b> Execute a store procedure and transforms the result to a List of the specified class entity.<br />
    Ex. <b>List</b><<b>MyClass</b>> results = <b>StoreProcedure</b>.Execute<<b>MyClass</b>>("dbo.GetTable1", <br />
	&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;new <b>Parameter</b>("@Param1", "Value"));
  * <b>ExecuteSingle:</b> Execute a store procedure and transforms only the first record of the result to the specified class entity.<br />
    Ex. <b>MyClass</b> result = <b>StoreProcedure</b>.ExecuteSingle<<b>MyClass</b>>("dbo.GetTable1", <br />
	&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;new <b>Parameter</b>("@Param1", "Value", DbType.String));
  * <b>ExecuteDataSet:</b> Execute a store procedure and returns the <b>DataSet</b> result with multiple tables.<br />
    Ex. <b>DataSet</b> results = <b>StoreProcedure</b>.ExecuteDataSet("dbo.GetTables;");
  * <b>ExecuteDataTable:</b> Execute a store procedure and returns the <b>DataTable</b> result.<br />
    Ex. <b>DataTable</b> results = <b>StoreProcedure</b>.ExecuteDataTable("dbo.GetTable1" });
  * <b>ExecuteDictionary:</b> Execute a store procedure and get a <b>Dictionary</b> specified with col0 as key and col1 as value.<br />
    Ex. <b>Dictionary</b><int,string> results = <b>StoreProcedure</b>.ExecuteDictionary<<b>Dictionary</b><int,string>>("dbo.GetAllTable1Dictionary");
  * <b>ExecuteScalar:</b> Execute a store procedure and get the scalar value.<br />
    Ex. <b>string</b> result = (<b>string</b>)<b>StoreProcedure</b>.ExecuteScalar("dbo.GetDescription", <br />
	  &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;new <b>Parameter</b>("@ID", 1, DbType.Int32));<br />
   &emsp;&nbsp;&nbsp; <b>string</b> result = <b>StoreProcedure</b>.ExecuteScalar<<b>string</b>>("dbo.GetDescription", <br />
	&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;new <b>Parameter</b>("@ID", 1, DbType.Int32));<br />
  * <b>ExecuteNonQuery:</b> Execute a store procedure.<br />
    Ex. <b>StoreProcedure</b>.ExecuteNonQuery("dbo.UpdateDescriptionByID", <br />
    &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;new <b>Parameter</b>("@Description", "Value", DbType.String),<br />
    &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;new <b>Parameter</b>("ID", 1, DbType.Int32));<br />

<b>Database Mapping</b> (<b>KuboEstudio.EF.Resources.Repository</b>)<b>:</b><br />
Description: Object-relational mapper.<br />
In order to use the following methods is necessary to add the approviate attributes to the class to use (please refer to the manual) and create a <b>IUnitOfWork</b> and <b>IRepository</b> object, which are created like the following:
  * <b>IUnitOfWork</b> unitOfWork = new <b>UnitOfWork</b>();
  * <b>IRepository</b><<b>MyClass</b>> repository = new <b>Repository</b><<b>MyClass</b>>(unitOfWork);<br />

Methods (please refer to the "UnitTestings and Examples"):<br />	
  * <b>Save:</b> Insert/Update the values to the entity to the table.<br />
    Ex. repository.Save(new <b>MyClass</b> { Description = "This is the description to save" });
  * <b>Find:</b> Get the entity based on the primary key of the table.<br />
    Ex. <b>MyClass</b> result = repository.Find(1);
  * <b>All:</b> Gets all the records in the table related to the class.<br />
    Ex. <b>List</b><<b>MyClass</b>> results = repository.All;
  * <b>Count:</b> Get a simple count based on the conditions specified.<br />
    Ex. <b>int</b> total = repository.Count();<br />
	Ex. <b>int</b> totalFiltered = repository.Count(new Restriction("Description", Condition.Like, "%Val%"));
  * <b>Where:</b> Gets the records that meet the restrictions.<br />
    Ex. <b>List</b><<b>MyClass</b>> results = repository.Where(new Restriction("Description", Condition.Like, "%Val%"));
  * <b>Delete:</b> Delete a record based on the primary key or an entity.<br />
    Ex. repository.Delete(1);<br />
	Ex. repository.Delete(myClassObj);
  * <b>SaveInMemory & DeleteOnMemory:</b> Creates a record in the unitOfWork for the inserts, updates and deletes to be execute all at the same time.<br />
    Ex. repository.SaveInMemory(new <b>MyClass</b> { Description = "This is another description to save" });<br />
	Ex. repository.DeleteOnMemory(1);<br />
	Ex. repository.DeleteOnMemory(myClassObj);<br /><br />
	<b>NOTES:</b> In order to execute the instructions you can only run the unitOfWork.Commit() or in case you don't want to execute anything you can do unitOfWork.Undo();<br />

<b>Azure Tables Mapping:</b> (<b>KuboEstudio.EF.Resources.AzureRepository</b>)<b>:</b><br />
Description: Azue Tables object-relational mapper.<br />
In order to use the following methods is necessary to add the approviate attributes to the class to use (please refer to the manual) and create a <b>IUnitOfWork</b> and <b>IRepository</b> object, which are created like the following:
  * <b>IUnitOfWork</b> unitOfWork = new <b>AzureUnitOfWork</b>(myCloudStorageAccount));
  * <b>IRepository</b><<b>MyClass</b>> repository = new <b>AzureRepository</b><<b>MyClass</b>>(unitOfWork);<br />

Methods (please refer to the "UnitTestings and Examples"):<br />	
  * <b>Save:</b> Insert/Update the values to the entity to the table.<br />
    Ex. repository.Save(new <b>MyClass</b> { Description = "This is the description to save" });
  * <b>Find:</b> Get the entity based on the primary key of the table.<br />
    Ex. <b>MyClass</b> result = repository.Find(1);
  * <b>All:</b> Gets all the records in the table related to the class.<br />
    Ex. <b>List</b><<b>MyClass</b>> results = repository.All;
  * <b>Count:</b> Get a simple count based on the conditions specified.<br />
    Ex. <b>int</b> total = repository.Count();<br />
	Ex. <b>int</b> totalFiltered = repository.Count(new Restriction("Description", Condition.Like, "%Val%"));
  * <b>Where:</b> Gets the records that meet the restrictions.<br />
    Ex. <b>List</b><<b>MyClass</b>> results = repository.Where(new Restriction("Description", Condition.Like, "%Val%"));
  * <b>Delete:</b> Delete a record based on the primary key or an entity.<br />
    Ex. repository.Delete(1);<br />
	Ex. repository.Delete(myClassObj);
  * <b>SaveInMemory & DeleteOnMemory:</b> Creates a record in the unitOfWork for the inserts, updates and deletes to be execute all at the same time.<br />
    Ex. repository.SaveInMemory(new <b>MyClass</b> { Description = "This is another description to save" });<br />
	Ex. repository.DeleteOnMemory(1);<br />
	Ex. repository.DeleteOnMemory(myClassObj);<br /><br />
	<b>NOTES:</b> In order to execute the instructions you can only run the unitOfWork.Commit() or in case you don't want to execute anything you can do unitOfWork.Undo();<br /><br />

#Class Definitions:
In order to use the following methods is necessary to add the approviate attributes to the class to use (please refer to the manual) and create a <b>IUnitOfWork</b> and <b>IRepository</b> object, which are created like the following:

  * <b>Database Mapping</b> (<b>KuboEstudio.EF.Schema.Attributes</b>)<b>:</b><br />
    In order to link the class to a table or view we have different attributes that can be use, which are the following:
    * <b>Class Attributes</b>:
      * <b>Table</b>
      	* Specifies the table related to the class.
      * <b>View</b>
      	* Specifies the view related to the class.
    * <b>Property Attributes</b>:
      * <b>PrimaryKey</b>
      	* Specifies that the property represent a PK column on the database.
      * <b>Identity</b>
      	* Specifies that the property represent an Identity column on the database.
      * <b>DBColumn</b>
      	* Specifies the real name of the column in the table specified in case the property name is not the same.
      	* <b>NOTE:</b> By default the name of the property is used as the name of the column
      * <b>NotDBColumn</b>
      	* Specifies that the property is not a column in the database.
      * <b>ForeignKey</b>
      	* Specifies that the property with this attribute is contains the primitive value related to ForeignProperty of the current object.
      * <b>ForeignProperty</b>
      	* Specifies that the property with this attribute is a parent of the current object.
      * <b>ChildForeignProperty</b>
      	* Specifies that the property with this attribute is a child of the current object.
      * <b>CascadeAll</b>
      	* Specifies that when save/delete the current object the <b>ChildForeignProperty</b> or <b>ForeignProperty</b> properties that contains this attribute will execute the same action.
      * <b>CascadeOnSave</b>
      	* Specifies that when save the current object the <b>ChildForeignProperty</b> or <b>ForeignProperty</b> properties that contains this attribute will execute the same action.
      * <b>CascadeOnDelete</b>
      	* Specifies that when delete the current object the <b>ChildForeignProperty</b> or <b>ForeignProperty</b> properties that contains this attribute will execute the same action.

  * <b>Azure Tables Mapping</b> (<b>KuboEstudio.EF.Azure.Schema.Attributes</b>)<b>:</b><br />
    In order to link the class to an azure table we have different attributes that can be use, which are the following:
    * <b>Class Attributes</b>:
      * <b>AzureTable</b>
      	* Specifies the table related to the class.
    * <b>Property Attributes</b>:
      * <b>AzureColumn</b>
      	* Specifies that the property is not a column in the database.
      	* <b>NOTE:</b> By default the name of the property is used as the name of the column.
      * <b>NotAzureColumn</b>
      	* Specifies that the property is not a column in the database.
      * <b>AzureForeignKey</b>
      	* Specifies that the property with this attribute is contains the primitive value related to AzureForeignProperty of the current object.
      * <b>AzureForeignProperty</b>
      	* Specifies that the property with this attribute is a parent of the current object.
      * <b>AzureChildForeignProperty</b>
      	* Specifies that the property with this attribute is a child of the current object.

# Helpers:

  * <b>Transform</b> (<b>KuboEstudio.EF.Resources.Transform</b>)<b>:</b><br />
	This will helps to transform a DataSet, DataTable or DataRow to an class entity. This will use the name of the Property as the name to relate with the DataColumn, in case you want that the Property name is different than the DataColumn name you use use the DBColumn attribute.
