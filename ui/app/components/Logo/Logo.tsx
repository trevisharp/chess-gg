export default function Logo() {
    return (
        <div>
            <svg width="420" height="120" viewBox="0 0 420 120" xmlns="http://www.w3.org/2000/svg" fill="none">
                <defs>
                    <linearGradient id="grad" x1="0" y1="0" x2="1" y2="1">
                        <stop offset="0%" stop-color="#6366F1"/>
                        <stop offset="100%" stop-color="#22C55E"/>
                    </linearGradient>

                    <pattern id="chessPattern" patternUnits="userSpaceOnUse" width="10" height="10">
                        <rect width="10" height="10" fill="#0f172a"/>
                        <rect width="5" height="5" fill="#1e293b"/>
                        <rect x="5" y="5" width="5" height="5" fill="#1e293b"/>
                    </pattern>
                </defs>

                <g transform="translate(10, 10)">
                    <rect x="0" y="0" width="100" height="100" rx="20" fill="url(#chessPattern)" />

                    <g transform="translate(25, 20)">
                        <path d="M25 0 L30 10 L40 10 L32 18 L35 30 L25 24 L15 30 L18 18 L10 10 L20 10 Z" fill="url(#grad)"/>
                        <rect x="20" y="30" width="10" height="25" rx="2" fill="url(#grad)"/>
                        <rect x="10" y="55" width="30" height="8" rx="2" fill="url(#grad)"/>
                        <rect x="5" y="63" width="40" height="10" rx="3" fill="url(#grad)"/>
                    </g>
                </g>

                <text x="130" y="65" font-family="Inter, Arial, sans-serif" font-size="42" font-weight="700" fill="#64748b">
                    chess<tspan fill="url(#grad)">GG</tspan>
                </text>

                <text x="132" y="90" font-family="Inter, Arial, sans-serif" font-size="14" fill="#64748b">
                    player analytics
                </text>
            </svg>
        </div>
    )
}