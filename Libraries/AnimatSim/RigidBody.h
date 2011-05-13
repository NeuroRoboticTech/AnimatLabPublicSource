/**
\file	RigidBody.h

\brief	Declares the rigid body class. 
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		class Odor;

		/**
		\class	RigidBody
		
		\brief	The base class for all of the basic rigid body type of objects.
		
		\details
		This class provides the base functionality for a rigid body.
		Each structure/Organism is made up of a heirarchcal tree of
		rigid bodies that are connected by Joint objects. The base
		structure has a root rigid body. That root body has a list of
		child rigid bodies that are connected to the root through a
		joint. Each of those child bodies can then have other
		children connected to them and so on.<br><br>
		Each rigid body within a structure/organism has a unique
		string identifier that is specifed for it.
		Whenever you are attempting to find a given part through a
		function like Simulator::FindRigidBody it is this
		identifier that is used as the key in the search. This class
		also contains a number of parameters that are common for all
		rigid bodies. These include things like the colors
		of the  object and the uniform density of the
		body. All rigid bodies have these properties.<br><br>
		This is a virtual base class and can not be directly created.
		You must override this class to provide specific
		functionality in order to create a real rigid body. For
		example, the Box and Cylinder are two examples of
		subclasses of a rigid body that add more parameters specific
		for their body type. Box has the x,y, and z widths for the
		box, whereas cylinder has the radius and height of the
		cylinder to create. The purpose of RigidBody and its
		subclasses like Box are to try and implement as
		much of the functionality as can be accomplished in this
		library, yet leave it flexible enough that a new physics
		engine could be easily swapped in for later use. This means that a
		lot of the mundane things like loading the data for the
		different rigid body and joint types is done for you
		automatically by the Body and Joint classes. But
		these classes can still not really implement all of the
		functionality necessary for the rigid body. The reason for
		this is that the overridable functions to create the actual
		implementation for those parts in your chosen physics engine
		must still be implemented. The two main functions related to
		that are CreateParts and CreateJoints. Since AnimatSim is
		generalized so that it is not tightly coupled to any one
		physics engine then that coupling must be done in the layer
		above this library where the chosen physics engine like
		Vortex is actually used.

		\author	dcofer
		\date	3/2/2011
		**/
		class ANIMAT_PORT RigidBody : public BodyPart    
		{
		protected:
			///The center of mass of this body relative to the center of the object. If this
			///is (0, 0, 0) then the default COM is used.
			CStdFPoint m_oCenterOfMass;

			///Specifies if the part should frozen in place to the world. If a rigid body 
			///is frozen then it is as if it is nailed in place and can not move. Gravity and 
			///and other forces will not act on it.
			BOOL m_bFreeze;

			///Uniform density for the rigid body.
			float m_fltDensity;

			///Drag Coefficient
			float m_vCd[3];   

			///Total volume for the rigid body. This is used in calculating the buoyancy
			float m_fltVolume;

			///The area of this rigid body in the x direction. This is used to calculate the
			///drag force in this direction.
			float m_fltXArea;

			///The area of this rigid body in the y direction. This is used to calculate the
			///drag force in this direction.
			float m_fltYArea;

			///The area of this rigid body in the z direction. This is used to calculate the
			///drag force in this direction.
			float m_fltZArea;

			///A list of child parts that are connected to this part through
			///different joints. 
			CStdPtrArray<RigidBody> m_aryChildParts;

			///A pointer to the joint that connects this rigid body to its parent.
			///If this is the root object then this pointer is null. However, All child parts
			///must be connected to their parent.
			Joint *m_lpJointToParent;

			///Some body parts like contact sensors and muscle attachments do not have joints. If not then we should not 
			///attempt to load them.
			BOOL m_bUsesJoint;

			///This determines whether or not this is a contact sensor. If this is TRUE then
			///this object does not take part in collisions and such, it is a contact sensor only.
			BOOL m_bIsContactSensor;

			///This determines whether the object is a collision geometry object
			BOOL m_bIsCollisionObject;

			///This keeps track of the current number of surface contacts that are occuring for this
			///contact sensor. This is only used for sensors.
			float m_fltSurfaceContactCount;

			/// The pointer to a receptive field ContactSensor object. This is responsible for 
			/// processing the receptive field contacts
			ContactSensor *m_lpContactSensor;

			/// The linear velocity damping for this body part.
			float m_fltLinearVelocityDamping;

			/// The angular velocity damping for this part.
			float m_fltAngularVelocityDamping;

			/// The array odor sources attached to this part.
			CStdPtrMap<string, Odor> m_aryOdorSources;
			
			///Tells if this body is considered a food source.
			BOOL m_bFoodSource;  
			
			/// The quantity of food that this part contains
			float m_fltFoodQuantity;

			///Tells how much food is being eaten.
			float m_fltFoodEaten;

			/// The maximum food quantity that this part can contain
			float m_fltMaxFoodQuantity;

			/// The rate at which food is replenished
			float m_fltFoodReplenishRate;

			/// The energy content of the food in calories.
			float m_fltFoodEnergyContent;

			/// Keeps track of how many time slices this part can eat.
			long m_lEatTime;

			/// Identifier for the material type this part will use.
			string m_strMaterialID;

			RigidBody *LoadRigidBody(CStdXml &oXml);
			Joint *LoadJoint(CStdXml &oXml);

			virtual void AddRigidBody(string strXml);
			virtual void RemoveRigidBody(string strID, BOOL bThrowError = TRUE);
			virtual int FindChildListPos(string strID, BOOL bThrowError = TRUE);

			Odor *LoadOdor(CStdXml &oXml);
			void AddOdor(Odor *lpOdor);

		public:
			RigidBody();
			virtual ~RigidBody();

			virtual void Position(CStdFPoint &oPoint, BOOL bUseScaling = TRUE, BOOL bFireChangeEvent = FALSE, BOOL bUpdateMatrix = TRUE);

			virtual int VisualSelectionType();

			virtual CStdFPoint CenterOfMass();
			virtual void CenterOfMass(CStdFPoint &oPoint);

			virtual CStdPtrArray<RigidBody>* ChildParts();

			virtual Joint *JointToParent();
			virtual void JointToParent(Joint *lpValue);

			virtual ContactSensor *ContactSensor();

			virtual float Density();
			virtual void Density(float fltVal);

			virtual float *Cd();
			virtual void Cd(float *vVal);

			virtual float Volume();
			virtual float XArea();
			virtual float YArea();
			virtual float ZArea();

			virtual BOOL Freeze();
			virtual void Freeze(BOOL bVal);

			virtual BOOL IsContactSensor();
			virtual void IsContactSensor(BOOL bVal);

			virtual BOOL IsCollisionObject();
			virtual void IsCollisionObject(BOOL bVal);

			virtual BOOL IsRoot();

			virtual BOOL IsFoodSource();
			virtual void IsFoodSource(BOOL bVal);

			virtual float FoodQuantity();
			virtual void FoodQuantity(float fltVal);

			virtual float FoodEaten();
			virtual void FoodEaten(float fltVal);
			
			virtual float FoodReplenishRate();
			virtual void FoodReplenishRate(float fltVal);

			virtual float FoodEnergyContent();
			virtual void FoodEnergyContent(float fltVal);

			virtual float LinearVelocityDamping();
			virtual void LinearVelocityDamping(float fltVal);

			virtual float AngularVelocityDamping();
			virtual void AngularVelocityDamping(float fltVal);

			virtual string MaterialID();
			virtual void MaterialID(string strID);

			virtual float SurfaceContactCount();

			virtual void Eat(float fltVal, long lTimeSlice);
			virtual void AddSurfaceContact(RigidBody *lpContactedSurface);
			virtual void RemoveSurfaceContact(RigidBody *lpContactedSurface);
			virtual void AddForce(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, BOOL bScaleUnits);
			virtual void AddTorque(float fltTx, float fltTy, float fltTz, BOOL bScaleUnits);
			virtual CStdFPoint GetVelocityAtPoint(float x, float y, float z);
			virtual float GetMass();
			virtual void UpdatePhysicsPosFromGraphics();

			virtual void EnableCollision(RigidBody *lpBody);
			virtual void DisableCollision(RigidBody *lpBody);

			virtual void CreateParts();
			virtual void CreateJoints();

#pragma region DataAccesMethods

			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
			virtual BOOL AddItem(string strItemType, string strXml, BOOL bThrowError = TRUE);
			virtual BOOL RemoveItem(string strItemType, string strID, BOOL bThrowError = TRUE);

#pragma endregion

			virtual void AddExternalNodeInput(float fltInput);
			virtual void StepSimulation();
			virtual void ResetSimulation();
			virtual void AfterResetSimulation();
			virtual void Kill(BOOL bState = TRUE);
			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
