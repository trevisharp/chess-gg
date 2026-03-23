import { useEffect, useState } from "react"
import { API_URL } from "~/config"
import type { AnalysisPageProps } from "./AnalysisPage.types"

export default function AnalysisPage({ player }: AnalysisPageProps) {
  const [playerData, setPlayerData] = useState<any>(null)

    const getPlayerData = async () => {
        try {
            const response = await fetch(API_URL + "/api/analysis/" + player)
            const json = await response.json()
            setPlayerData(json)
        }
        catch (err) {
            console.log(err)
        }
    }

    useEffect(() => {
        getPlayerData()
    }, [])

    return (
        <div>
            { playerData ? 
                    <div>
                        
                    </div>
                : <h1>Loading...</h1>
            }
        </div>
    )
}