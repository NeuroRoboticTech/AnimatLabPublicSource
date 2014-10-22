
-- A solution contains projects, and defines the available configurations
solution "AnimatCarlSimCUDA"
	configurations { "Debug", "Release" }
	dofile "Projects.lua"
