import React from "react";
import Image from "next/image";

import ShipmentDataDashboard from "@components/Cards/Shipments/ShipmentDataDashboard";
import ShipmentLoadingDashboard from "@components/Cards/Shipments/ShipmentLoadingDashboard";
import ShipmentDetailsDashboard from "@components/Cards/ShipmentDetails/ShipmentDetailsDashboard";
import ShipmentDetailsLoadingDashboard from "@components/Cards/ShipmentDetails/ShipmentDetailsLoadingDashboard";

export default function Dashboard({ params, searchParams }) {
  const shipmentNumber = searchParams.shipment;

  return (
    <>
      <div className="flex flex-row h-[86vh]">
        <div className="contents">
          <React.Suspense fallback={<ShipmentLoadingDashboard />}>
            <div className="max-w-[26rem] min-h-full flex-initial mr-4 overflow-x-scroll">
              <ShipmentDataDashboard />
            </div>
          </React.Suspense>
          {/* <div className="w-auto min-h-full flex-1 border-2 rounded-lg border-gray-300 dark:border-gray-600 p-4 mb-4 overflow-x-scroll">
            SHipment Conconte
            <br />
            {shipmentNumber}
          </div> */}
          <React.Suspense fallback={<ShipmentDetailsLoadingDashboard />}>
            <div className="w-auto min-h-full flex-1 border-2 rounded-lg border-gray-300 dark:border-gray-600 p-4 mb-4 overflow-x-scroll">
              {shipmentNumber ? (
                <>
                  <div className="flex h-full">
                    <ShipmentDetailsDashboard id={shipmentNumber} />
                  </div>
                </>
              ) : (
                <>
                  <div
                    className="flex flex-col justify-center items-center h-full"
                    style={{
                      backgroundImage: "url('/world.svg')",
                      backgroundRepeat: "no-repeat",
                      backgroundSize: "contain",
                      backgroundPositionY: "center",
                    }}
                  >
                    <p>Shipment Content</p>
                    <br />
                    <p>Select Any Shipment to view the data.</p>
                    <br />
                    <p>{shipmentNumber}</p>
                  </div>
                </>
              )}
            </div>
          </React.Suspense>
        </div>
      </div>
    </>
  );
}
