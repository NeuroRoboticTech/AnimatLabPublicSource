import sys,os,traceback
sys.path.append("C:/Projects/AnimatLabSDK/AnimatLabPublicSource/bin")
os.chdir("C:/Projects/AnimatLabSDK/AnimatLabPublicSource/bin")
import AnimatSimPy
simmgr = AnimatSimPy.SimulationMgr.Instance()
simthread = simmgr.CreateSimulation("C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Tutorials\Examples\StandAloneSimTest\Bullet_Single_x32_Debug.asim")
simthread = simmgr.CreateSimulation("C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Libraries\RoboticsAnimatSim\Robotics_UnitTests\TestResources\BasicHingeTest\BasicHingeTest_Win32_Robotics.asim")
simthread.Simulate(2)
simmgr.ShutdownAllSimulations()


elbow = AnimatSimPy.MotorizedJoint.CastToDerived(AnimatSimPy.ActiveSim().FindByID("0854af91-cef9-48ce-9dba-545daf873a15"))
elbow = AnimatSimPy.MotorizedJoint.CastToDerived(AnimatSimPy.ActiveSim().FindByID("ba1616eb-45ad-48b4-aee4-f47c6d19ba09"))
