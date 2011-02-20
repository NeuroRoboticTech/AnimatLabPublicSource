#ifndef __[*TAG_NAME*]_CONSTANTS_H__
#define __[*TAG_NAME*]_CONSTANTS_H__

#define Nl_Err_lInvalidNeuronType -3000
#define Nl_Err_strInvalidNeuronType "Invalid neuron type."

#define Nl_Err_lInvalidSynapseType -3001
#define Nl_Err_strInvalidSynapseType "Invalid synapse type."

#define Nl_Err_lSimNotDefined -3002
#define Nl_Err_strSimNotDefined "The simulation is not defined."

#define Nl_Err_lClassFactoryNotDefined -3003
#define Nl_Err_strClassFactoryNotDefined "The class factory was not defined."

#define Nl_Err_lInvalidOrganismType -3004
#define Nl_Err_strInvalidOrganismType "Invalid organism type."

#define Nl_Err_lOrganismNotDefined -3005
#define Nl_Err_strOrganismNotDefined "Organism not defined."

#define Nl_Err_lInvalidSizeSpec -3006
#define Nl_Err_strInvalidSizeSpec "Invalid Size specification."

#define Nl_Err_lNeuronToSetNull -3007
#define Nl_Err_strNeuronToSetNull "The neuron to set was null."

#define Nl_Err_lNervousSystemNotDefined -3008
#define Nl_Err_strNervousSystemNotDefined "Brain has not been defined."

#define Nl_Err_lInvalidNeuronDataType -3009
#define Nl_Err_strInvalidNeuronDataType "Invalid neuron data type was specified."

#define Nl_Err_lInjectionToAddNull -3010
#define Nl_Err_strInjectionToAddNull "Injection to be added is null."

#define Nl_Err_lSensoryMotorPointerNotFound -3011
#define Nl_Err_strSensoryMotorPointerNotFound "No sensory motor pointer was found for the specified ID."

#define Nl_Err_lInvalidGain -3012
#define Nl_Err_strInvalidGain "An invalid gain has been specified. Gain must be non-zero."

#define Nl_Err_lSynapseToAddNull -3013
#define Nl_Err_strSynapseToAddNull "Synapse to be added to the Neuron is null."

#define Nl_Err_lInvalidSynapseIndex -3014
#define Nl_Err_strInvalidSynapseIndex "An invalid Synapse index value was specified."

#define Nl_Err_lCompoundNotSupported -3015
#define Nl_Err_strCompoundNotSupported "Compound synapse is not supported for this synapse type."

#define Nl_Err_lInvalidGraphType -3016
#define Nl_Err_strInvalidGraphType "Invalid graph type."

#define Nl_Err_lProjectPathBlank -3017
#define Nl_Err_strProjectPathBlank "Project path is blank."

#define Nl_Err_lNeuralNetworkBlank -3018
#define Nl_Err_strNeuralNetworkBlank "Neural network file is blank."

#define Nl_Err_lFilenameBlank -3019
#define Nl_Err_strFilenameBlank "Filename is blank."

#define Nl_Err_lIDBlank -3020
#define Nl_Err_strIDBlank "ID is blank."

#define Nl_Err_lInvalidDataType -3022
#define Nl_Err_strInvalidDataType "Invalid data type."

#define Nl_Err_lInvalidDataChartType -3023
#define Nl_Err_strInvalidDataChartType "Invalid data chart type."

#define Nl_Err_lSimulationNotDefined -3024
#define Nl_Err_strSimulationNotDefined "Simulation has not been defined."

#define Nl_Err_lExceededMaxBuffer -3025
#define Nl_Err_strExceededMaxBuffer "Data Buffer size has been exceeded."

#define Nl_Err_lItemNotDataChartType -3026
#define Nl_Err_strItemNotDataChartType "The activated object is not a data chart type."

#define Nl_Err_lInvalidExternalStimulusType -3027
#define Nl_Err_strInvalidExternalStimulusType "Invalid external stimulus type."

#define Nl_Err_lItemNotStimulusType -3028
#define Nl_Err_strItemNotStimulusType "Activated item is not an external stimulus type."

#define Nl_Err_lNeuronToInjectNotFound -3029
#define Nl_Err_strNeuronToInjectNotFound "The neuron to inject was not found."

#define Nl_Err_lNeuronNotFound -3030
#define Nl_Err_strNeuronNotFound "The specified neuron was not found."

#define Nl_Err_lActivatedItemIDNotFound -3031
#define Nl_Err_strActivatedItemIDNotFound "The specified activated item was not found."

//Neuron Types
#define RUGULAR_NEURON 1

//Synapse Types
#define REGULAR_SYNAPSE 1

#ifndef PI
	#define PI 3.14159
	#define PI_2 6.28319
#endif

#endif // __[*TAG_NAME*]_CONSTANTS_H__
