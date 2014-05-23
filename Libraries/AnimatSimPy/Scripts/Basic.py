import os
import sys
sys.path.append("C:/Projects/AnimatLabSDK/AnimatLabPublicSource/bin")
os.chdir("C:/Projects/AnimatLabSDK/AnimatLabPublicSource/bin")
import AnimatSimPy
simmgr = AnimatSimPy.SimulationMgr()
simthread = simmgr.CreateSimulation("C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Tutorials\Examples\StandAloneSimTest\Bullet_Single_x32.asim")
simthread.Simulate(2)
simmgr.ShutdownAllSimulations()

