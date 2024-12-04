local edb = GetModData().Buildings
local buildingCount = edb:GetCount()

---@class recruitedUnit
---@field unit string
---@field cityPools table<integer, number> 
---@field castlePools table<integer, number> 
---@field bothPools table<integer, number>
recruitedUnit = {}

function recruitedUnit:new(o)
    o = o or {}
    o.unit = o.unit or ""
    o.cityPools = o.cityPools or {}
    o.castlePools = o.castlePools or {}
    o.bothPools = o.bothPools or {}
    setmetatable(o, self)
    self.__index = self
    return o
end

---@type table<string, recruitedUnit>
RECRUIT_UNITs = {}

function getPoolData()
    print("Start getting pool data..")
    for i = 0, buildingCount - 1 do
        local building = edb:GetByIndex(i)
        if building then
            for j = 0, building.LevelCount - 1 do
                local level = building.Levels[j]
                for c = 0, level.CapabilityCount - 1 do
                    local capability = level.Capabilities[c]
                    if capability.Type == "recruit_pool" and capability.MaximumPool >= 1 then
                        local unit = capability.Unit
                        if not RECRUIT_UNITs[unit] then
                            RECRUIT_UNITs[unit] = recruitedUnit:new({unit = unit})
                        end
                        if level.AvailableCastle and level.AvailableCity then
                            table.insert(RECRUIT_UNITs[unit].bothPools, capability.ReplenishmentRate)
                        elseif level.AvailableCastle then
                            table.insert(RECRUIT_UNITs[unit].castlePools, capability.ReplenishmentRate)
                        else 
                            table.insert(RECRUIT_UNITs[unit].cityPools, capability.ReplenishmentRate)
                        end
                    end
                end
            end
        end
    end
    print("Finish getting pool data..")
end

function makeCSV()
    print("Start Making csv..")
    local csv = io.open("recruit_pool.csv", "w")
    if not csv then
        print("Cant open file")
        return
    end
    csv:write("Unit;Average;AverageCity;AverageCastle;Highest;Lowest\n")
    for k, v in pairs(RECRUIT_UNITs) do
        local both = 0
        local city = 0
        local castle = 0
        local bothCount = 0
        local cityCount = 0
        local castleCount = 0
        local highest = 0
        local lowest = 9999
        for i = 1, #v.bothPools do
            both = both + v.bothPools[i]
            bothCount = bothCount + 1
            if v.bothPools[i] > highest then
                highest = v.bothPools[i]
            end
            if v.bothPools[i] < lowest then
                lowest = v.bothPools[i]
            end
        end
        for i = 1, #v.cityPools do
            city = city + v.cityPools[i]
            cityCount = cityCount + 1
            if v.cityPools[i] > highest then
                highest = v.cityPools[i]
            end
            if v.cityPools[i] < lowest then
                lowest = v.cityPools[i]
            end
        end
        for i = 1, #v.castlePools do
            castle = castle + v.castlePools[i]
            castleCount = castleCount + 1
            if v.castlePools[i] > highest then
                highest = v.castlePools[i]
            end
            if v.castlePools[i] < lowest then
                lowest = v.castlePools[i]
            end
        end
        local average = (both + city + castle) / (bothCount + cityCount + castleCount)
        local averageCity = 0
        if cityCount > 0 then
            averageCity = city / cityCount
        end
        local averageCastle = 0
        if castleCount > 0 then
            averageCastle = castle / castleCount
        end
        csv:write(k..";"..average..";"..averageCity..";"..averageCastle..";"..highest..";"..lowest.."\n")
    end
    csv:flush()
    csv:close()
    print("Finish Making csv..")
end

getPoolData()
makeCSV()
