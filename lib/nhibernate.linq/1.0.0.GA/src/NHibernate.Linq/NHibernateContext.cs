using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Services;
using System.Linq;
using System.Reflection;
using NHibernate.Metadata;
namespace System.Data.Services
{
	//
	// Summary:
	//     This interface declares the methods required to support the $expand query option
	//     for an WCF Data Services.
	public interface IExpandProvider
	{
		//
		// Summary:
		//     Applies expansions to the specified queryable parameter.
		//
		// Parameters:
		//   queryable:
		//     The System.Linq.IQueryable`1 object to expand.
		//
		//   expandPaths:
		//     A collection of System.Data.Services.ExpandSegmentCollection paths to expand.
		//
		// Returns:
		//     An System.Collections.IEnumerable object of the same type as the supplied queryable
		//     object that includes the specified expandPaths.
		IEnumerable ApplyExpansions(IQueryable queryable, ICollection<ExpandSegmentCollection> expandPaths);
	}
}
namespace System.Data.Services
{
	//
	// Summary:
	//     An interface used to insert or update a resource by the HTTP POST method.
	public interface IUpdatable
	{
		//
		// Summary:
		//     Adds the specified value to the collection.
		//
		// Parameters:
		//   targetResource:
		//     Target object that defines the property.
		//
		//   propertyName:
		//     The name of the collection property to which the resource should be added.
		//
		//   resourceToBeAdded:
		//     The opaque object representing the resource to be added.
		void AddReferenceToCollection(object targetResource, string propertyName, object resourceToBeAdded);
		//
		// Summary:
		//     Cancels a change to the data.
		void ClearChanges();
		//
		// Summary:
		//     Creates the resource of the specified type and that belongs to the specified
		//     container.
		//
		// Parameters:
		//   containerName:
		//     The name of the entity set to which the resource belongs.
		//
		//   fullTypeName:
		//     The full namespace-qualified type name of the resource.
		//
		// Returns:
		//     The object representing a resource of specified type and belonging to the specified
		//     container.
		object CreateResource(string containerName, string fullTypeName);
		//
		// Summary:
		//     Deletes the specified resource.
		//
		// Parameters:
		//   targetResource:
		//     The resource to be deleted.
		void DeleteResource(object targetResource);
		//
		// Summary:
		//     Gets the resource of the specified type identified by a query and type name.
		//
		// Parameters:
		//   query:
		//     Language integrated query (LINQ) pointing to a particular resource.
		//
		//   fullTypeName:
		//     The fully qualified type name of resource.
		//
		// Returns:
		//     An opaque object representing a resource of the specified type, referenced by
		//     the specified query.
		object GetResource(IQueryable query, string fullTypeName);
		//
		// Summary:
		//     Gets the value of the specified property on the target object.
		//
		// Parameters:
		//   targetResource:
		//     An opaque object that represents a resource.
		//
		//   propertyName:
		//     The name of the property whose value needs to be retrieved.
		//
		// Returns:
		//     The value of the object.
		object GetValue(object targetResource, string propertyName);
		//
		// Summary:
		//     Removes the specified value from the collection.
		//
		// Parameters:
		//   targetResource:
		//     The target object that defines the property.
		//
		//   propertyName:
		//     The name of the property whose value needs to be updated.
		//
		//   resourceToBeRemoved:
		//     The property value that needs to be removed.
		void RemoveReferenceFromCollection(object targetResource, string propertyName, object resourceToBeRemoved);
		//
		// Summary:
		//     Resets the resource identified by the parameter resource to its default value.
		//
		// Parameters:
		//   resource:
		//     The resource to be updated.
		//
		// Returns:
		//     The resource with its value reset to the default value.
		object ResetResource(object resource);
		//
		// Summary:
		//     Returns the instance of the resource represented by the specified resource object.
		//
		// Parameters:
		//   resource:
		//     The object representing the resource whose instance needs to be retrieved.
		//
		// Returns:
		//     The instance of the resource represented by the specified resource object.
		object ResolveResource(object resource);
		//
		// Summary:
		//     Saves all the changes that have been made by using the System.Data.Services.IUpdatable
		//     APIs.
		void SaveChanges();
		//
		// Summary:
		//     Sets the value of the specified reference property on the target object.
		//
		// Parameters:
		//   targetResource:
		//     The target object that defines the property.
		//
		//   propertyName:
		//     The name of the property whose value needs to be updated.
		//
		//   propertyValue:
		//     The property value to be updated.
		void SetReference(object targetResource, string propertyName, object propertyValue);
		//
		// Summary:
		//     Sets the value of the property with the specified name on the target resource
		//     to the specified property value.
		//
		// Parameters:
		//   targetResource:
		//     The target object that defines the property.
		//
		//   propertyName:
		//     The name of the property whose value needs to be updated.
		//
		//   propertyValue:
		//     The property value for update.
		void SetValue(object targetResource, string propertyName, object propertyValue);
	}
}
namespace NHibernate.Linq
{
	/// <summary>
	/// Wraps an <see cref="T:NHibernate.ISession"/> object to provide base functionality
	/// for custom, database-specific context classes.
	/// </summary>
	public abstract class NHibernateContext : IDisposable, ICloneable, IUpdatable, IExpandProvider
	{
		/// <summary>
		/// Provides access to database provider specific methods.
		/// </summary>
		public readonly IDbMethods Methods;

		private ISession session;

		/// <summary>
		/// Initializes a new instance of the <see cref="NHibernateContext"/> class.
		/// </summary>
		public NHibernateContext()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:NHibernate.Linq.NHibernateContext"/> class.
		/// </summary>
		/// <param name="session">An initialized <see cref="T:NHibernate.ISession"/> object.</param>
		public NHibernateContext(ISession session)
		{
			this.session = session;
		}

		/// <summary>
		/// Gets a reference to the <see cref="T:NHibernate.ISession"/> associated with this object.
		/// </summary>
		public virtual ISession Session
		{
			get
			{
				if (session == null)
				{
					// Attempt to get the Session
					session = ProvideSession();
				}
				return session;
			}
		}



		/// <summary>
		/// Allows for empty construction but provides an interface for an interface to have the derived 
		/// classes provide a session object late in the cycle. 
		/// </summary>
		/// <returns>The Required <see cref="T:NHibernate.ISession"/> object.</returns>
		protected virtual ISession ProvideSession()
		{
			// Should not be called as supplying the session in the constructor
			throw new NotImplementedException("If NHibernateContext is constructed with the empty constructor, inheritor is required to override ProvideSession to supply Session.");
		}

		#region ICloneable Members

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns></returns>
		public virtual object Clone()
		{
			if (session == null)
			{
				throw new ArgumentNullException("session");
			}

			return Activator.CreateInstance(GetType(), session);
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Disposes the wrapped <see cref="T:NHibernate.ISession"/> object.
		/// </summary>
		public virtual void Dispose()
		{
			if (session != null)
			{
				session.Dispose();
				session = null;
			}
		}

		#endregion

		#region IUpdatable Members

		List<object> _updateCache = null;
		/// <summary>
		/// Gets the update cache.
		/// </summary>
		/// <value>The update cache.</value>
		List<object> UpdateCache
		{
			get
			{
				if (_updateCache == null)
				{
					_updateCache = new List<object>();
				}
				return _updateCache;
			}
		}

		/// <summary>
		/// Adds the reference to collection.
		/// </summary>
		/// <param name="targetResource">The target resource.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="resourceToBeAdded">The resource to be added.</param>
		void IUpdatable.AddReferenceToCollection(object targetResource, string propertyName, object resourceToBeAdded)
		{
			IClassMetadata metadata = session.SessionFactory.GetClassMetadata(targetResource.GetType().FullName);
			if (metadata == null)
			{
				throw new DataServiceException("Type not recognized as a valid type for this Context");
			}

			// Get the property to use to add the resource to
			object collection = metadata.GetPropertyValue(targetResource, propertyName);

			// Try with IList implementation first (its faster)
			if (collection is IList)
			{
				((IList)collection).Add(resourceToBeAdded);
			}
			else // Try with Reflection's Add()
			{
				MethodInfo addMethod = collection.GetType().GetMethod("Add", BindingFlags.Public | BindingFlags.Instance);
				if (addMethod == null)
				{
					throw new DataServiceException(string.Concat("Could not determine the collection type of the ", propertyName, " property."));
				}
				addMethod.Invoke(collection, new object[] { resourceToBeAdded });
			}
		}

		/// <summary>
		/// Clears the changes.
		/// </summary>
		void IUpdatable.ClearChanges()
		{
			UpdateCache.Clear();
			session.Clear();
		}

		/// <summary>
		/// Creates the resource.
		/// </summary>
		/// <param name="containerName">Name of the container.</param>
		/// <param name="fullTypeName">Full name of the type.</param>
		/// <returns>Newly created Resource</returns>
		object IUpdatable.CreateResource(string containerName, string fullTypeName)
		{
			// Get the metadata
			IClassMetadata metadata = session.SessionFactory.GetClassMetadata(fullTypeName);
			object newResource = metadata.Instantiate(null);

			// We can't save it to the session as it may not be valid yet
			// This happens if the key is a non-initancable key (e.g. Northwind.Customers)
			// So we save them to a local cache.  Only when SaveAll happens will be push them to the Session
			UpdateCache.Add(newResource);

			// Returns the new resource 
			return newResource;
		}

		/// <summary>
		/// Deletes the resource.
		/// </summary>
		/// <param name="targetResource">The target resource.</param>
		void IUpdatable.DeleteResource(object targetResource)
		{
			// Push it to the Session to support deletion
			if (UpdateCache.Contains(targetResource))
			{
				UpdateCache.Remove(targetResource);
				session.Save(targetResource);
			}

			// Mark it as deleted
			if (session.Contains(targetResource)) session.Delete(targetResource);
		}

		/// <summary>
		/// Gets the resource.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <param name="fullTypeName">Full name of the type.</param>
		/// <returns></returns>
		object IUpdatable.GetResource(System.Linq.IQueryable query, string fullTypeName)
		{
			// Get the first result
			IEnumerable results = (IEnumerable)query;
			object returnValue = null;
			foreach (object result in results)
			{
				if (returnValue != null) break;
				returnValue = result;
			}

			// Check the Typename if needed
			if (fullTypeName != null)
			{
				if (fullTypeName != returnValue.GetType().FullName)
				{
					throw new DataServiceException("Incorrect Type Returned");
				}
			}

			// Return the resource
			return returnValue;
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <param name="targetResource">The target resource.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns></returns>
		object IUpdatable.GetValue(object targetResource, string propertyName)
		{
			IClassMetadata metadata = session.SessionFactory.GetClassMetadata(targetResource.GetType().FullName);
			if (metadata == null)
			{
				throw new DataServiceException("Type not recognized as a valid type for this Context");
			}

			// If 
			if (metadata.IdentifierPropertyName == propertyName)
			{
				return metadata.GetIdentifier(targetResource);
			}
			else
			{
				return metadata.GetPropertyValue(targetResource, propertyName);
			}
		}

		/// <summary>
		/// Removes the reference from collection.
		/// </summary>
		/// <param name="targetResource">The target resource.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="resourceToBeRemoved">The resource to be removed.</param>
		void IUpdatable.RemoveReferenceFromCollection(object targetResource, string propertyName, object resourceToBeRemoved)
		{
			IClassMetadata metadata = session.SessionFactory.GetClassMetadata(targetResource.GetType().FullName);
			if (metadata == null)
			{
				throw new DataServiceException("Type not recognized as a valid type for this Context");
			}

			// Get the property to use to remove the resource to
			object collection = metadata.GetPropertyValue(targetResource, propertyName);

			// Try with IList implementation first (its faster)
			if (collection is IList)
			{
				((IList)collection).Remove(resourceToBeRemoved);
			}
			else // Try with Reflection's Add()
			{
				MethodInfo removeMethod = collection.GetType().GetMethod("Remove", BindingFlags.Public | BindingFlags.Instance);
				if (removeMethod == null)
				{				
					throw new DataServiceException(string.Concat("Could not determine the collection type of the ", propertyName, " property."));
				}
				removeMethod.Invoke(collection, new object[] { resourceToBeRemoved });
			}
		}

		/// <summary>
		/// Replaces the resource.
		/// </summary>
		/// <param name="resource">The resource to reset.</param>
		/// <returns></returns>
		object IUpdatable.ResetResource(object resource)
		{
			IUpdatable update = this;

			// Create a new resource of the same type
			// but only make a local copy as we're only using it to set the default fields
			// Get the metadata
			IClassMetadata metadata = session.SessionFactory.GetClassMetadata(resource.GetType().ToString());
			object tempCopy = metadata.Instantiate(null);

			// Copy the default non-keys
			foreach (string propName in metadata.PropertyNames)
			{
				object value = metadata.GetPropertyValue(tempCopy, propName);
				update.SetValue(resource, propName, value);
			}

			//Return the new resource
			return resource;
		}

		/// <summary>
		/// Resolves the resource.
		/// </summary>
		/// <param name="resource">The resource.</param>
		/// <returns></returns>
		object IUpdatable.ResolveResource(object resource)
		{
			// Resolve Resource always just returns the resource
			// since we not using tokens or cookies to the actual objects
			// the resources are always the actual CLR objects
			return resource;
		}

		/// <summary>
		/// Saves the changes.
		/// </summary>
		void IUpdatable.SaveChanges()
		{
			// All saves must be all or nothing.
			using (ITransaction tx = Session.BeginTransaction())
			{
				try
				{
					// If we have anything in the object cache,
					// add it to session.
					if (_updateCache != null)
					{
						_updateCache.ForEach(o => session.SaveOrUpdate(o));
						_updateCache.Clear();
					}

					// Push the changes to the database
					session.Flush();

					// Commit the Transaction
					tx.Commit();
				}
				catch (Exception ex)
				{
					// If anythign goes wrong, it all gets rolled back
					tx.Rollback();

					// Send the error back to the user
					throw new DataException("Failed to save changes.  See inner exception for details", ex);
				}
			}
		}

		/// <summary>
		/// Sets the reference.
		/// </summary>
		/// <param name="targetResource">The target resource.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="propertyValue">The property value.</param>
		void IUpdatable.SetReference(object targetResource, string propertyName, object propertyValue)
		{
			((IUpdatable)this).SetValue(targetResource, propertyName, propertyValue);
		}

		/// <summary>
		/// Sets the value.
		/// </summary>
		/// <param name="targetResource">The target resource.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="propertyValue">The property value.</param>
		void IUpdatable.SetValue(object targetResource, string propertyName, object propertyValue)
		{
			IClassMetadata metadata = session.SessionFactory.GetClassMetadata(targetResource.GetType().FullName);
			if (metadata == null)
			{
				throw new DataServiceException("Type not recognized as a valid type for this Context");
			}

			// See if its the Key property first
			if (metadata.IdentifierPropertyName == propertyName)
			{
				metadata.SetIdentifier(targetResource, propertyValue);
			}
			else // Else set the property
			{
				metadata.SetPropertyValue(targetResource, propertyName, propertyValue);
			}
		}

		#endregion

		#region IExpandProvider Members

		IEnumerable IExpandProvider.ApplyExpansions(IQueryable queryable, ICollection<ExpandSegmentCollection> expandPaths)
		{
			if (queryable == null) throw new DataServiceException("Query cannot be null");

			INHibernateQueryable nHibQuery = queryable as INHibernateQueryable;
			if (nHibQuery == null) throw new DataServiceException("Expansion only supported on INHibernateQueryable queries");

			if (expandPaths.Count == 0) throw new DataServiceException("Expansion Paths cannot be null");
			foreach (ExpandSegmentCollection coll in expandPaths)
			{
				foreach (ExpandSegment seg in coll)
				{
					if (seg.HasFilter)
					{
						throw new DataServiceException("NHibernate does not support Expansions with Filters");
					}
					else
					{
						nHibQuery.QueryOptions.AddExpansion(seg.Name);
					}
				}
			}

			return nHibQuery as IEnumerable;
		}

		#endregion
	}
}