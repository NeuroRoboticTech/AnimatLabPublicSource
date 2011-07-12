/**
\file	Simulator.h

\brief	Declares the simulator class.
**/

#pragma once

#include "MaterialPair.h"
#include "Materials.h"
#include "SimulationWindow.h"
#include "HudItem.h"
#include "Hud.h"
#include "SimulationWindowMgr.h"

namespace AnimatSim
{
		/**
		\brief	Simulates the entire environment.

		\details This is the core simulation object of AnimatLab. It is responsible for managing all organisms/structures, the environment, etc..
		Essentially, anything related to the simulation is controlled by this class, or the derived class that is specific for a physics engine.
		
		\author	dcofer
		\date	3/25/2011
		**/
		class ANIMAT_PORT Simulator : public AnimatBase
		{
		protected:

#pragma region Variables

#pragma region ProjectVariables

			///The directory path where the simulation configuration files are located.
			string m_strProjectPath;

			///The directory path where the executable is located.
			string m_strExecutablePath;

			///The name of the Animat Simulation (ASIM) file.
			string m_strSimulationFile;

			///A list of all Organisms in this simulation. This is not a reference list
			///It is an actual list that destroys its objects when the list is destroyed.
			CStdPtrMap<string, Organism> m_aryOrganisms;

			///A list of structures in this simulation that are not organisms. 
			///This is not a reference list. It is an actual list that destroys
			///its objects when the list is destroyed.
			CStdPtrMap<string, Structure> m_aryStructures;

			///A list of all structures, both regular structures and organisms. This is
			///a reference list.
			CStdMap<string, Structure *> m_aryAllStructures;

			///A list of all odors in this simulation. This is not a reference list
			///It is an actual list that destroys its objects when the list is destroyed.
			CStdPtrMap<string, OdorType> m_aryOdorTypes;

			/// An Array of RigidBody parts that also act as food sources within the environment.
			CStdArray<RigidBody *> m_aryFoodSources;

			///A list of class factories for neural modules that have been loaded by any organisms in the
			///environment. Each factory is only added to the list once even if the 
			///the same module is loaded seperately for each organism. 
			CStdMap<string, IStdClassFactory *> m_aryNeuralModuleFactories;

			///This is a copy of the class factory associated with the animat module. 
			///This is the default class factory that is used if no specific module name
			///is specified. If a specific name is given then it looks through the list
			///of modules that were loaded for neural modules. 
			///All class factories should have have NO state.
			IStdClassFactory *m_lpAnimatClassFactory;

			///A list of all animatbase objects in the simulation. 
			CStdMap<string, AnimatBase *> m_aryObjectList;

			/// Array of source physics adapters
			CStdArray<Adapter *> m_arySourcePhysicsAdapters;

			/// Array of target physics adapters. 
			CStdArray<Adapter *> m_aryTargetPhysicsAdapters;

			/// Number of target adapters
			int m_iTargetAdapterCount;

			/// Manager for data charts
			DataChartMgr m_oDataChartMgr;

			/// Manager for external stimuli
			ExternalStimuliMgr m_oExternalStimuliMgr;

			/// The pointer to a simulation recorder
			SimulationRecorder *m_lpSimRecorder;

			/// Manager for Materials
			Materials m_oMaterialMgr;

			/// Manager for SimulationWindows
			SimulationWindowMgr *m_lpWinMgr;

			/// Manager for the light objects.
			LightManager m_oLightMgr;

			///This is the currect visual selection mode used within the GUI.
			int m_iSelectionMode;

			/// true if the AddBodies mode is enabled within the GUI.
			BOOL m_bAddBodiesMode;

			/// true to block simulation. See WaitForBlock for more info.
			BOOL m_bBlockSimulation;

			/// true to confirm that a simulation block is in place.
			BOOL m_bSimBlockConfirm;

			/// Pointer to a simulation callback class. This is only set if we are using the GUI. This is set by the managed wrapper to 
			/// allow us to post callback notifications to the GUI.
			ISimGUICallback *m_lpSimCallback;

			/// Set to true to stop the simulation.
			BOOL m_bStopSimulation;

			/// Set to true to stop the simulation. This is a more forceful way of stopping the sim.
			BOOL m_bForceSimulationStop;

			/// If true then the user is manually stepping the simulation
			BOOL m_bManualStepSimulation;

			/// true if the simulation is paused.
			BOOL m_bPaused;

			/// true if the simulation has been initialized.
			BOOL m_bInitialized;

			/// true if the simulation is running
			BOOL m_bSimRunning;

			/// true if we need to set the physics system to force handling of fast moving objects.
			BOOL m_bForceFastMoving;

#pragma endregion

#pragma region UnitScalingVariables
			
			/// Tells how many meters each unit of distance is worth within the simulation environment.
			float m_fltDistanceUnits;

			/// The inverse of the distance units
			float m_fltInverseDistanceUnits;

			/// For items that use distance unit measures in the denominator we may want to use a differnt
			/// scale that that used for the whole app. For example, if we are using a distance scale of decimeters
			/// we will want to use centimeters for the density instead. This allows us to do that.
			float m_fltDenominatorDistanceUnits;

			/// Tells how many kilograms each unit of mass is worth within the simulation environment.
			float m_fltMassUnits;

			/// The inverse of the mass units
			float m_fltInverseMassUnits;

			/// Tells how many kilograms/m^3 each unit of density is worth within the simulation environment.
			float m_fltDensityMassUnits;

#pragma endregion

#pragma region EnvironmentVariables

			///The acceleration of gravity to use in the simulation.
			float m_fltGravity;

			/// The linear compliance of the simulation environment.
			float m_fltLinearCompliance;

			/// The angular compliance of the simulation environment.
			float m_fltAngularCompliance;

			/// The linear damping of the simulation environment.
			float m_fltLinearDamping;

			/// The angular damping of the simulation environment.
			float m_fltAngularDamping;

			/// The linear kinetic loss of the simulation environment.
			float m_fltLinearKineticLoss;

			/// The angular kinetic loss of the simulation environment.
			float m_fltAngularKineticLoss;

			///Tells whether or not we will be doing hydrodynamic simulations.
			///If you are not doing stuff underwater then be sure this is set to
			///FALSE. The hydrodynamics adds extra overhead that can slow the
			///simulation down slightly.
			BOOL m_bSimulateHydrodynamics;

			///This is the minimum integration time step taken for all neural modules and the physics engine.
			float m_fltTimeStep;

			///The current time slice. This a long value.
			long m_lTimeSlice;

			///The current simulation time.
			float m_fltTime;

			/// true if we the user has manually set the simulation end time.
			BOOL m_bSetEndSim;

			///The time when the simulation should end.
			float m_fltEndSimTime;

			///The time slice when the simulation should end
			float m_lEndSimTimeSlice;

			///The tick count for when the simulation first begins running.
			DWORD m_iStartSimTick;

			/// Zero-based index of the frame rate
			int m_iFrameRate;

			/// The time value duration of the frame step.
			float m_fltFrameStep;

			///The number of slices that the physics engine has been updated.
			long m_lPhysicsSliceCount;

			///This tells how many time slices occur between each interval where the 
			///body physics is updated. There could be multiple neural updates in between
			///one physics update.
			short m_iPhysicsStepInterval;

			///The time increment for each time slice of the physics simulation. 
			float m_fltPhysicsTimeStep;

			///This keeps track of the number of steps since the physics system was
			///last updated. By using this we do not have to do a bunch of divisions
			///at each time step to determine if it is time to update the physics engine.
			int m_iPhysicsStepCount;

			///Determines whether a random seed is automatically generated when the 
			///simulation is initialized, or if a manual seed is used.
			BOOL m_bAutoGenerateRandomSeed;

			///If the AutoGenerateRandomSeed variable is false then this manual seed is
			///used for the random number generator.
			int m_iManualRandomSeed;
			
			/// The stiffness of the user mouse spring.
			float m_fltMouseSpringStiffness;

			/// The damping of the user mouse spring.
			float m_ftlMouseSpringDamping;

			/// The background color to use when drawing the environment.
			CStdColor m_vBackgroundColor;

			/// The radius of the sphere shown for a receptive field. This is used in the selection code
			/// so we can show the selected vertex. It adds a sphere to the part at that vertex coordinate and
			/// uses this radius when drawing the sphere.
			float m_fltRecFieldSelRadius;

#pragma endregion

#pragma region RecordingVariables

			/// true to enable simulation recording
			BOOL m_bEnableSimRecording;

			/// Size of a memory snapshot in bytes.
			long m_lSnapshotByteSize;

			/// Current video slice time slice
			long m_lVideoSliceCount;

			/// Zero-based index of the video loops
			int m_iVideoLoops;

			/// The pointer to a video recorder
			KeyFrame *m_lpVideoRecorder;

			/// The pointer to a video playback
			KeyFrame *m_lpVideoPlayback;

			/// The pointer to a simulation stop point
			KeyFrame *m_lpSimStopPoint;

			//Video variables
			/// true to record video
			BOOL m_bRecordVideo;

			/// Filename of the video file
			string m_strVideoFilename;

			/// Time of the video record frame
			float m_fltVideoRecordFrameTime;

			/// Time of the video playback frame
			float m_fltVideoPlaybackFrameTime;

			/// Time of the video start
			float m_fltVideoStartTime;

			/// Time of the video end
			float m_fltVideoEndTime;

			/// Size of the i video step
			int m_iVideoStepSize;

			/// Zero-based index of the video step
			int m_iVideoStep;

			/// The video start slice
			long m_lVideoStartSlice;

			/// The video end slice
			long m_lVideoEndSlice;

			/// The video frame
			long m_lVideoFrame;

			/// The pointer to an avi
			CStdAvi *m_lpAvi;

			/// Options for controlling the avi
			AVICOMPRESSOPTIONS m_aviOpts; 

#pragma endregion

#pragma region TempVariables
			//These variables are just used on and off. They are defined here so we do not have to keep redefining them.

			/// An organism iterator
			CStdMap<string, Organism *>::iterator m_oOrganismIterator;

			/// A structure iterator
			CStdMap<string, Structure *>::iterator m_oStructureIterator;

			/// The pointer to a selected organism
			Organism *m_lpSelOrganism;

			/// The pointer to a selected structure
			Structure *m_lpSelStructure;

#pragma endregion

#pragma endregion

#pragma region ProtectedMethods

#pragma region LoadMethods

			virtual void LoadEnvironment(CStdXml &oXml);
			Structure *LoadStructure(CStdXml &oXml);
			Organism *LoadOrganism(CStdXml &oXml);
			OdorType *LoadOdorType(CStdXml &oXml);

			static void LoadAnimatModuleName(string strFile, string &strAnimatModule);
			static void LoadAnimatModuleName(CStdXml &oXml, string &strAnimatModule);

#pragma endregion

#pragma region AddRemoveMethods

			/**
			\brief	Adds a new organism to the list of structures for this simulation.

			\details This method gets a list of all organisms and a list of referneces to
			all structures in this simulation that are mapped to their
			ID value. This allows us to use the STL find funtions to find organisms.
			This is more efficeient that using a loop and recursion.
			This also allows us to ensure that each organism/structure that is
			being added has a unique ID value. If you attempt to add a organism that
			has a ID that is already in the list then an exception will be thrown.
			Note that this method is NOT creating the object itself, that is done
			elsewhere. It is simply adding it to the organism list and adding 
			a reference to that created object to m_aryAllStructures list.

			\author	dcofer
			\date	3/28/2011

			\param [in,out]	lpOrganism	Pointer to an organism. 
			**/
			void AddOrganism(Organism *lpOrganism);
			void AddOrganism(string strXml);
			void RemoveOrganism(string strID, BOOL bThrowError = TRUE);

			/**
			\brief	Adds a new "static" structure to the list of structures for this simulation.

			\author	dcofer
			\date	3/28/2011

			\param [in,out]	lpStructure	Pointer to the structure to add. 
			**/
			void AddStructure(Structure *lpStructure);
			void AddStructure(string strXml);
			void RemoveStructure(string strID, BOOL bThrowError = TRUE);

			void AddOdorType(OdorType *lpOdorType);

#pragma endregion

#pragma region UnitScaleMethods

			float ConvertDistanceUnits(string strUnits);
			float ConvertDenominatorDistanceUnits(string strUnits);
			float ConvertMassUnits(string strUnits);
			float ConvertDensityMassUnits(string strUnits);

#pragma endregion

#pragma region SimulationMethods

			virtual void InitializeStructures();

			virtual BOOL CheckSimulationBlock();
			virtual void CheckEndSimulationTime();

			virtual void StepNeuralEngine();
			virtual void StepPhysicsEngine();
			virtual void Step();

			virtual void ProcessSimulationStep();
			virtual void StepSimulation();
			virtual void StepVideoFrame() = 0;
			virtual void SimulateBegin();
			virtual void SimulateEnd();

			//These functions are called internally when the simulation is about to start up or pause.
			virtual void SimStarting();
			virtual void SimPausing();
			virtual void SimStopping();
			
			virtual void GenerateAutoSeed();

			virtual void HandleCriticalError(string strError);
			virtual void HandleNonCriticalError(string strError);

#pragma endregion

#pragma region RecorderMethods

			/**
			\brief	Creates the simulation recorder.
			
			\author	dcofer
			\date	3/28/2011
			
			\return	Pointer to the SimulationRecorder.
			**/
			virtual SimulationRecorder *CreateSimulationRecorder() = 0;

			virtual long CalculateSnapshotByteSize();

			/**
			\brief	Takes a snapshot of the current frame.
			
			\author	dcofer
			\date	3/28/2011
			**/
			virtual void SnapshotStopFrame() = 0;

			virtual void RecordVideoFrame();

#pragma endregion

#pragma endregion

		public:
			Simulator();
			virtual ~Simulator();

#pragma region AccessorMutators
			
#pragma region ProjectVariables

			virtual string ProjectPath();
			virtual void ProjectPath(string strPath);

			virtual string ExecutablePath();
			virtual void ExecutablePath(string strPath);

			virtual string SimulationFile();
			virtual void SimulationFile(string strFile);

			virtual BOOL Paused();
			virtual void Paused(BOOL bVal);

			virtual BOOL Initialized();
			virtual void Initialized(BOOL bVal);

			virtual CStdMap<string, AnimatBase *> *ObjectList();
			
			virtual DataChartMgr *DataChartMgr();
			virtual ExternalStimuliMgr *ExternalStimuliMgr();
			virtual SimulationRecorder *SimulationRecorder();
			virtual Materials *MaterialMgr();
			virtual SimulationWindowMgr *WindowMgr();
			virtual LightManager *LightMgr();

			virtual int VisualSelectionMode();
			virtual void VisualSelectionMode(int iVal);

			virtual BOOL AddBodiesMode();
			virtual void AddBodiesMode(BOOL bVal);
			
			virtual	ISimGUICallback *SimCallback();
			virtual void SimCallBack(ISimGUICallback *lpCallback);

#pragma endregion

#pragma region EnvironmentVariables

			virtual float Time();
			virtual long Millisecond();
			virtual long MillisecondToSlice(long lMillisecond);
			virtual long SliceToMillisecond(long lSlice);
			virtual DWORD StartSimTick();

			virtual long TimeSlice();
			virtual void TimeSlice(long lVal);

			virtual long PhysicsSliceCount();
			virtual void PhysicsSliceCount(long lVal);

			virtual BOOL ManualStepSimulation();
			virtual void ManualStepSimulation(BOOL bVal);

			virtual BOOL SimRunning();

			virtual BOOL ForceFastMoving();
			virtual void ForceFastMoving(BOOL bVal);

			virtual BOOL AutoGenerateRandomSeed();
			virtual void AutoGenerateRandomSeed(BOOL bVal);

			virtual int ManualRandomSeed();
			virtual void ManualRandomSeed(int iSeed);

			virtual float LinearCompliance();
			virtual void LinearCompliance(float fltVal, BOOL bUseScaling = TRUE);

			virtual float AngularCompliance();
			virtual void AngularCompliance(float fltVal, BOOL bUseScaling = TRUE);

			virtual float LinearDamping();
			virtual void LinearDamping(float fltVal, BOOL bUseScaling = TRUE);

			virtual float AngularDamping();
			virtual void AngularDamping(float fltVal, BOOL bUseScaling = TRUE);

			virtual float LinearKineticLoss();
			virtual void LinearKineticLoss(float fltVal);

			virtual float AngularKineticLoss();
			virtual void AngularKineticLoss(float fltVal);

			virtual float TimeStep();
			virtual void TimeStep(float fltVal);

			virtual BOOL SetEndSimTime();
			virtual void SetEndSimTime(BOOL bVal);

			virtual float EndSimTime();
			virtual void EndSimTime(float fltVal);

			virtual long EndSimTimeSlice();
			virtual void EndSimTimeSlice(long lVal);

			virtual BOOL Stopped();

			virtual int FrameRate();
			virtual float FrameStep();
			virtual void FrameRate(int iVal);
			virtual short PhysicsStepInterval();
			virtual void PhysicsStepInterval(short iVal);
		 
			virtual void PhysicsTimeStep(float fltVal);
			virtual float PhysicsTimeStep();
			virtual long PhysicsStepCount();

			virtual float Gravity();
			virtual void Gravity(float fltVal, BOOL bUseScaling = TRUE);

			virtual float MouseSpringStiffness();
			virtual void MouseSpringStiffness(float fltVal, BOOL bUseScaling = TRUE);

			virtual float MouseSpringDamping();
			virtual void MouseSpringDamping(float fltVal, BOOL bUseScaling = TRUE);

			virtual BOOL SimulateHydrodynamics();
			virtual void SimulateHydrodynamics(BOOL bVal);

			virtual int GetMaterialID(string strID);

			virtual BOOL IsPhysicsBeingUpdated();	
			
			virtual CStdColor *BackgroundColor();
			virtual void BackgroundColor(float *vColor);

			virtual float RecFieldSelRadius();
			virtual void RecFieldSelRadius(float fltValue, BOOL bUseScaling = TRUE, BOOL bUpdateAllBodies = TRUE);

#pragma endregion

#pragma region UnitScalingVariables
			
			virtual void DistanceUnits(string strUnits);
			virtual float DistanceUnits();
			virtual float InverseDistanceUnits();
			virtual float DenominatorDistanceUnits();

			virtual void MassUnits(string strUnits);
			virtual float MassUnits();
			virtual float InverseMassUnits();
			virtual float DensityMassUnits();

#pragma endregion
			
#pragma region RecordingVariables
			
			virtual long VideoSliceCount();
			virtual void VideoSliceCount(long lVal);

			virtual int VideoLoops();
			virtual void VideoLoops(int iVal);

			/**
			\brief	Gets the video recorder.

			\author	dcofer
			\date	3/28/2011

			\return	Pointer to the video recorder.
			**/
			virtual KeyFrame *VideoRecorder();

			/**
			\brief	Sets the video recorder.

			\author	dcofer
			\date	3/28/2011

			\param [in,out]	lpFrame	Pointer to a frame. 
			**/
			virtual void VideoRecorder(KeyFrame *lpFrame);

			/**
			\brief	Gets the video playback frame.

			\author	dcofer
			\date	3/28/2011

			\return	Pointer to the video playback frame.
			**/
			virtual KeyFrame *VideoPlayback();

			/**
			\brief	Sets the video playback frame.

			\author	dcofer
			\date	3/28/2011

			\param [in,out]	lpFrame	Pointer to the video playback frame. 
			**/
			virtual void VideoPlayback(KeyFrame *lpFrame);

			virtual BOOL EnableSimRecording();
			virtual void EnableSimRecording(BOOL bVal);

			virtual long SnapshotByteSize();

#pragma endregion

#pragma endregion

#pragma region PublicMethods

#pragma region SimulationMethods

			virtual void BlockSimulation();
			virtual void UnblockSimulation();
			virtual BOOL SimulationBlockConfirm();
			virtual BOOL WaitForSimulationBlock(long lTimeout = -1);

			virtual void Reset(); //Resets the entire application back to the default state 
			virtual void ResetSimulation(); //Resets the current simulation back to time 0.0

			/**
			\brief	Initializes this object.
			
			\author	dcofer
			\date	3/28/2011

			\details This is a pure virtual method that must be implemented in the simulator application.
			 It is where a lot of the nitty gritty details are done with initializing and 
			 setting up the physics engine so that it can run. It is also where we initialize
			 each structure to tell them to create their parts and joints.

			\param	argc	The argc parameter from the command line. 
			\param	argv	The argv parameter from the command line. 
			**/
			virtual void Initialize(int argc, const char **argv) = 0;

			/**
			\brief	Simulates the system.

			\details This starts the simulation running. This method does not return until the simulation 
			has been stopped. It is a blocking call. It loops through and calls the Step method repeatedly.
			
			\author	dcofer
			\date	3/28/2011
			**/
			virtual void Simulate();

			/**
			\brief	Shuts down the simulation.
			
			\author	dcofer
			\date	3/28/2011
			**/
			virtual void ShutdownSimulation() = 0;

			/**
			\brief	Toggles the simulation between running and paused.
			
			\author	dcofer
			\date	3/28/2011
			**/
			virtual void ToggleSimulation() = 0;

			/**
			\brief	Stops the simulation and resets it.
			
			\author	dcofer
			\date	3/28/2011
			**/
			virtual void StopSimulation() = 0;

			/**
			\brief	Starts the simulation.
			
			\author	dcofer
			\date	3/28/2011
			
			\return	true if it succeeds, false if it fails.
			**/
			virtual BOOL StartSimulation() = 0;

			/**
			\brief	Pauses the simulation.
			
			\author	dcofer
			\date	3/28/2011
			
			\return	true if it succeeds, false if it fails.
			**/
			virtual BOOL PauseSimulation() = 0;

			/**
			\brief	Runs the simulation.

			\details This is different from the Simulate method. This is used when the AnimatSimulator is running standalone.
			It Loads the simulation from the specified file, initailizes it, and the calls Simulate.
			
			\author	dcofer
			\date	3/28/2011
			**/
			virtual void RunSimulation();

#pragma endregion
			
#pragma region LoadMethods

			virtual void Load(string strFileName = "");
			virtual void Load(CStdXml &oXml);
			virtual void Save(string strFilename);

			static IStdClassFactory *LoadClassFactory(string strModuleName);

#pragma endregion
		 			
#pragma region CreateMethods

			virtual CStdSerialize *CreateObject(string strModule, string strClassName, string strType, BOOL bThrowError = TRUE);
			static Simulator *CreateSimulator(string strSimulationFile);
			static Simulator *CreateSimulator(CStdXml &oXml);
			static Simulator *CreateSimulator(int argc, const char **argv);
			virtual void GenerateCollisionMeshFile(string strOriginalMeshFile, string strCollisionMeshFile) = 0;

#pragma endregion
		 
	
#pragma region FindMethods

			virtual IStdClassFactory *FindNeuralModuleFactory(string strModuleName, BOOL bThrowError = FALSE);
			virtual Organism *FindOrganism(string strOrganismID, BOOL bThrowError = TRUE);
			virtual Structure *FindStructure(string strStructureID, BOOL bThrowError = TRUE);
			virtual Structure *FindStructureFromAll(string strStructureID, BOOL bThrowError = TRUE);
			virtual Joint *FindJoint(string strStructureID, string strJointID, BOOL bThrowError = TRUE);
			virtual RigidBody *FindRigidBody(string strStructureID, string strBodyID, BOOL bThrowError = TRUE);
			virtual OdorType *FindOdorType(string strOdorID, BOOL bThrowError = TRUE);
			virtual RigidBody *FindClosestFoodSource(CStdFPoint &oMouthPos, float fltMinRadius);
			virtual AnimatBase *FindByID(string strID, BOOL bThrowError = TRUE);

#pragma endregion

#pragma region AddRemoveMethods

			virtual void AddToObjectList(AnimatBase *lpItem);
			virtual void RemoveFromObjectList(AnimatBase *lpItem);
			virtual void AddNeuralModuleFactory(string strModuleName, NeuralModule *lpModule);

			virtual void AddFoodSource(RigidBody *lpFood);

			virtual void AttachSourceAdapter(Structure *lpStructure, Adapter *lpAdapter);
			virtual void AttachTargetAdapter(Structure *lpStructure, Adapter *lpAdapter);

#pragma endregion

#pragma region DataAccesMethods

			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
			virtual BOOL AddItem(string strItemType, string strXml, BOOL bThrowError = TRUE);
			virtual BOOL RemoveItem(string strItemType, string strID, BOOL bThrowError = TRUE);

#pragma endregion

#pragma region RecorderMethods

			virtual void EnableVideoPlayback(string strKeyFrameID);
			virtual void DisableVideoPlayback();
			virtual void StartVideoPlayback();
			virtual void StopVideoPlayback();
			virtual void StepVideoPlayback(int iFrameCount = 1);
			virtual void SaveVideo(string strPath);

			virtual string AddKeyFrame(string strType, long lStart, long lEnd);
			virtual void RemoveKeyFrame(string strID);
			virtual string MoveKeyFrame(string strID, long lStart, long lEnd);
			virtual void MoveSimulationToKeyFrame(string strKeyFrameID);
			virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
			virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);

#pragma endregion
			
#pragma region CollisionMethods

			virtual void EnableCollisions(Structure *lpStruct, CStdPtrArray<CollisionPair> &m_aryCollisionList);
			virtual void DisableCollisions(Structure *lpStruct, CStdPtrArray<CollisionPair> &m_aryCollisionList);

			/**
			\brief	Enables collision between the past-in body and all rigid bodies of the simulator.

			\remarks This method enables collision responses between the rigid body being past
			in and all rigid bodies in the simulator.

			\author	dcofer
			\date	3/28/2011

			\param [in,out]	lpBody	Pointer to a body. 
			**/
			virtual void EnableCollision(RigidBody *lpBody);

			/**
			\brief	Disables collision between the past-in body and all rigid bodies of the simulator.

			\remarks This method disables collision responses between the rigid body being past
			in and all rigid bodies in the simulator.

			\author	dcofer
			\date	3/28/2011

			\param [in,out]	lpBody	Pointer to a body. 
			**/
			virtual void DisableCollision(RigidBody *lpBody);

#pragma endregion

#pragma endregion

		};

		Simulator ANIMAT_PORT *GetSimulator();

}			//AnimatSim
