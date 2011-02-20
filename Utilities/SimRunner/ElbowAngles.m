clear;

iIndex = 1;
for(iTrial=5:5:100)
    %set the filename
    strFile = sprintf('C:\\Program Files\\AnimatLabSDK\\VS7\\Utilities\\SimRunner\\Results\\ArmFlex_I%d_Toolviewer_1.txt', iTrial);     

    %read in the data from the text file
    [TimeSlice Time Angles BicepLengths TricepLengths StMns SbMns BicepTensions TricepTensions] = textread(strFile,'%d %f %f %f %f %f %f %f %f', 'headerlines',1);

    %find the array index where the time is 7 seconds
    iTimeIdx = find(Time == 7.0); 
    
    %get the elbow angle at 7 seconds
    ElbowAngle(iIndex) = Angles(iTimeIdx);
    Current(iIndex) = iTrial*0.01;
    iIndex = iIndex+1;
end

%fit a line through our data
FitVals = polyfit(Current, ElbowAngle, 1);
FitCurrent = 0:0.01:1;
AnglesPred = polyval(FitVals, FitCurrent);

%plot our data
hold off;
plot(Current, ElbowAngle, 'o', 'MarkerEdgeColor', 'b', 'MarkerFaceColor', 'w');
hold on;
plot(FitCurrent, AnglesPred, 'k');

